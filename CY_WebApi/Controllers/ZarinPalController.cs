//using CY_WebApi.Services;
//using Microsoft.AspNetCore.Mvc;
//using CY_BM;
//using CY_WebApi.Models;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
//using System.Security.Policy;
//using System.Text;
//using System.Net.Http;
//using static Google.Cloud.RecaptchaEnterprise.V1.AccountVerificationInfo.Types;
//using System.Security.Claims;
//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ZarinPalController : ControllerBase
//    {
//        private readonly CyContext _db;
//        private readonly HttpClient _httpClient;

//        private readonly string merchantId = "df72fda6-1b01-4214-bf60-cdeeb458fd99";

//        public ZarinPalController(CyContext db)
//        {
//            _db = db;
//            _httpClient = new HttpClient();
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet("pay")]
//        public async Task<ActionResult> Pay(int orderId)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var order = _db.CyOrder.Where(o=> o.IsVisible && o.ID == orderId && o.CyUserID == userid).Include(o=> o.CyOrderItems).FirstOrDefault();
//            if (order == null) { return NotFound(); }

//            var amount = order.CyOrderItems.Sum(o => o.TotalPrice);  
//            order.TotalAmount = amount;
//            order.OrderDate = DateTime.Now;
//            await _db.SaveChangesAsync();

            

//            string description = "خرید از صانع";

//            var url = "https://payment.zarinpal.com/pg/v4/payment/request.json";

//            var requestBody = new
//            {
//                merchant_id = merchantId,
//                amount,
//                callback_url = "https://saneComputer.com/paymentResult/" + order.ID,
//                description,
//            };

//            var jsonContent = new StringContent(
//                System.Text.Json.JsonSerializer.Serialize(requestBody),
//                Encoding.UTF8,
//                "application/json"
//            );

//            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

//            try
//            {
//                var response = await _httpClient.PostAsync(url, jsonContent);
//                var responseContent = await response.Content.ReadAsStringAsync();
//                var jsonResponse = JsonConvert.DeserializeObject<ZarianPalResponseDTO>(responseContent);

//                if (response.IsSuccessStatusCode)
//                {
//                    //order.Status = OrderStatus.Pending;
//                    //order.LastModified = DateTime.Now;
//                    //await _db.SaveChangesAsync();

//                    var authority = jsonResponse?.Data.Authority;
//                    return Ok( new { Url = "https://payment.zarinpal.com/pg/StartPay/" + authority });
//                }
//                else
//                {
//                    return BadRequest(jsonResponse?.Data);
//                }
//            }
//            catch (HttpRequestException ex)
//            {
//                return StatusCode(500, $"Error occurred while making the request: {ex.Message}");
//            }
//        }


//        [TypeAuthorize([UserType.Customer])]
//        [HttpPost("varifyPay")]
//        public async Task<ActionResult> VarifyPay([FromBody] PaymentVerifyRequest request)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var order = _db.CyOrder.Where(o => o.IsVisible && o.ID == request.OrderId && o.CyUserID == userid).Include(o => o.CyOrderItems).FirstOrDefault();
//            if (order == null) { return NotFound(); }

//            var amount = order.CyOrderItems.Sum(o => o.TotalPrice);

//            var url = "https://payment.zarinpal.com/pg/v4/payment/verify.json";

//            // Prepare the JSON payload
//            var jsonPayload = new
//            {
//                merchant_id = merchantId,
//                amount,
//                authority = request.Authority
//            };
//            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(jsonPayload), Encoding.UTF8, "application/json");

//            // Send the POST request
//            var response = await _httpClient.PostAsync(url, content);
//            var responseContent = await response.Content.ReadAsStringAsync();
//            var jsonResponse = JsonConvert.DeserializeObject<ZarianPalResponseDTO>(responseContent);

//            if (response.IsSuccessStatusCode)
//            {
//                order.Status = OrderStatus.Finalized;
//                order.LastModified = DateTime.Now;
//                order.FinalizedDate = DateTime.Now;
//                await _db.SaveChangesAsync();

//                return Ok(jsonResponse?.Data);
//            }

//            return BadRequest(jsonResponse?.Data);
//        }
//    }
//}
