using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mall_Management.Models;
using System.Net.Mail;

namespace Mall_Management.Controllers
{
    public class ContractsController : Controller
    {
        private mall_dbEntities db = new mall_dbEntities();

        // GET: Contracts
        public async Task<ActionResult> Index()
        {
            var contracts = db.Contracts.Include(c => c.Account).Include(c => c.Space);
            return View(await contracts.ToListAsync());
        }

        // GET: Contracts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // GET: Contracts/Create
        public ActionResult Create()
        {
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username");
            ViewBag.SpaceID = new SelectList(db.Spaces, "SpaceID", "SpaceName");
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ContractID,SpaceID,AccountID,StartDate,EndDate,RentAmount,Status")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                db.Contracts.Add(contract);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username", contract.AccountID);
            ViewBag.SpaceID = new SelectList(db.Spaces, "SpaceID", "SpaceName", contract.SpaceID);
            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username", contract.AccountID);
            ViewBag.SpaceID = new SelectList(db.Spaces, "SpaceID", "SpaceName", contract.SpaceID);
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ContractID,SpaceID,AccountID,StartDate,EndDate,RentAmount,Status")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contract).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username", contract.AccountID);
            ViewBag.SpaceID = new SelectList(db.Spaces, "SpaceID", "SpaceName", contract.SpaceID);
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Contract contract = await db.Contracts.FindAsync(id);
            db.Contracts.Remove(contract);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public JsonResult UpdateStatus(int contractId, string status)
        {
            try
            {
                // Tìm hợp đồng theo ContractID
                var contract = db.Contracts.FirstOrDefault(c => c.ContractID == contractId);
                if (contract == null)
                {
                    return Json(new { success = false, message = "Hợp đồng không tồn tại!" });
                }

                // Lấy tài khoản dựa vào AccountID từ bảng Contracts
                var account = db.Accounts.FirstOrDefault(a => a.AccountID == contract.AccountID);
                if (account == null)
                {
                    return Json(new { success = false, message = "Tài khoản không tồn tại!" });
                }

                // Lấy email của người dùng
                string email = account.Email;
                string newStatus = null;

                // Kiểm tra trạng thái và cập nhật
                if (contract.Status == "Pending" && status == "Active")
                {
                    contract.Status = "Active";
                    newStatus = "Active";
                }
                else if (contract.Status == "Active" && status == "Terminated")
                {
                    contract.Status = "Terminated";
                    newStatus = "Terminated";

                    // Lấy Space liên kết với Contract và cập nhật trạng thái
                    var space = db.Spaces.FirstOrDefault(s => s.SpaceID == contract.SpaceID);
                    if (space != null)
                    {
                        space.Status = "Available";
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Không thể thay đổi trạng thái theo hướng này." });
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                // Gửi email thông báo
                SendStatusChangeEmail(email, contract.ContractID, newStatus);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        private void SendStatusChangeEmail(string email, int contractId, string newStatus)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("luonggiabao060904@gmail.com"), // Email gửi
                    Subject = "Cập nhật trạng thái hợp đồng",
                    Body = $"Kính chào bạn,<br><br>Hợp đồng có ID {contractId} của bạn đã được cập nhật trạng thái mới: <strong>{newStatus}</strong>.<br><br>Trân trọng,<br>Đội ngũ quản lý.",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential("luonggiabao060904@gmail.com", "cqpo mdjt sflr bpzg"); // Thay "your-app-password" bằng mật khẩu ứng dụng
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gửi email thất bại: {ex.Message}");
            }
        }


    }
}
