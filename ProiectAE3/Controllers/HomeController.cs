using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using PayPal.Api;
using System;
using System.Collections.Generic;

namespace ProiectAE3.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;
        private Payment payment;

        public HomeController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentWithPayPal(string cancel = null, string blogId = "", string payerId = "", string guid = "")
        {
            try
            {
                var clientId = configuration.GetValue<string>("PayPal:Key");
                var clientSecret = configuration.GetValue<string>("PayPal:Secret");
                var mode = configuration.GetValue<string>("PayPal:mode");
                var apiContext = PayPalConfiguration.GetAPIContext(clientId, clientSecret, mode);

                if (string.IsNullOrWhiteSpace(payerId))
                {
                    var baseURI = $"{this.Request.Scheme}://{this.Request.Host}/Home/PaymentWithPayPal?";
                    var guidd = Convert.ToString((new Random()).Next(100000));
                    guid = guidd;

                    var createdPayment = CreatePayment(apiContext, $"{baseURI}guid={guid}", blogId);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        var lnk = links.Current;
                        if (lnk.rel?.ToLower().Trim() == "approval_url")
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    if (paypalRedirectUrl != null)
                    {
                        httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
                        return Redirect(paypalRedirectUrl);
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Approval URL not found in PayPal response.";
                        return View("PaymentFailed");
                    }
                }
                else
                {
                    var paymentId = httpContextAccessor.HttpContext.Session.GetString("payment");
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId);

                    if (executedPayment?.state?.ToLower() != "approved")
                    {
                        ViewData["ErrorMessage"] = "Payment was not approved.";
                        return View("PaymentFailed");
                    }

                    var blogs = executedPayment.transactions?[0]?.item_list?.items?[0]?.sku;

                    return View("PaymentSuccess");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                ViewData["ErrorMessage"] = "An error occurred during payment processing.";
                return View("PaymentFailed");
            }
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution
            {
                payer_id = payerId,
            };

            if (payment != null)
            {
                payment.id = paymentId;
                return payment.Execute(apiContext, paymentExecution);
            }
            else
            {
                throw new InvalidOperationException("Payment is null in ExecutePayment method.");
            }
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId)
        {
            var itemList = new ItemList
            {
                items = new List<Item>
                {
                    new Item
                    {
                        name = "Item Detail",
                        currency = "EUR",
                        price = "1.00",
                        quantity = "1",
                        sku = "asd"
                    }
                }
            };

            var payer = new Payer
            {
                payment_method = "paypal"
            };

            var redirectUrls = new RedirectUrls
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var amount = new Amount
            {
                currency = "EUR",
                total = "1.00"
            };

            var transactionList = new List<Transaction>
            {
                new Transaction
                {
                    description = "Transaction description",
                    invoice_number = Guid.NewGuid().ToString(),
                    amount = amount,
                    item_list = itemList
                }
            };

            payment = new Payment
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirectUrls
            };

            return payment.Create(apiContext);
        }
    }
}