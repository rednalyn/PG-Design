﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using pgDesign.dbEngine;
using pgDesign.ViewModels;
using pgDesign.Models;
using System.Threading.Tasks;

namespace pgDesign.Controllers
{
    public class AdminController : Controller
    {
        private dbOperation db;
        private StartUpViewModel vm;
        private ContactVM Cvm;
        private AzureBlobHelper AB;
        private postedFileModel pfm;
        private Webshop ws;
        public AdminController()
        {
            db = new dbOperation();
            vm = new StartUpViewModel();
            Cvm = new ContactVM();
            AB = new AzureBlobHelper();
            pfm = new postedFileModel();
            ws = new Webshop();

    }
        // GET: Admin
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var SiteInfoAboutText = db.SiteinfoText(1);
                vm.AboutText = SiteInfoAboutText.Content;
                vm.Id = SiteInfoAboutText.Id;

                var SiteInfoOpen = db.SiteinfoText(2);
                vm.OpenTimes = SiteInfoOpen.Content;

                string ContainerName = "carouselpictures";

                vm.Bloblist = AB.GetListOfData(pfm, ContainerName);

                return View(vm);
            }
        }
        public ActionResult GalleryAdmin()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        public ActionResult ContactAdmin()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Cvm.Users = db.GetUsers();
                return View(Cvm);
            }
        }
        [HttpPost]
        public ActionResult SetContactAdmin(ContactInfo ci)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                try
                {
                    db.SetContactInfo(ci);
                    TempData["Message"] = "<br />Uppgifterna har nu blivit sparade.";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "<br /> Det har uppstått ett fel <br /> <br />" + ex;
                }
                return RedirectToAction("ContactAdmin");
            }
        }

        [HttpPost]
        public ActionResult AboutText(StartUpViewModel sv)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                db.SetAboutText(sv);
                return RedirectToAction("Index");
            }
        }
        public ActionResult GetListOfAccounts()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var list = db.GetAllUsers();
                return View(list);
            }
        }
        public ActionResult Edit(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var user = db.GetSpeifikUser(id);
                return View(user);
            }
        }
        public ActionResult WebshopAdminList()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
               var list = db.GetWebshops();
                return View(list);
            }
        }
        public ActionResult CreateWebshopItem()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {


                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateWebshopItem(Webshop ws, HttpPostedFileBase file)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                    }
                }
                var imageUrl = await AB.UploadBlobtest(file);

                db.CreateWebshopItem(ws, imageUrl.ToString());

                return RedirectToAction("WebshopAdminList");
            }
        }
        public ActionResult EditWebshopItem(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var ws = db.GetSpecificWebshop(id);

                return View(ws);
            }
        }
        [ActionName("EditItem")]
        public async Task<ActionResult> EditWebshopItem(Webshop ws, HttpPostedFileBase file)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                    }
                }
                var imageUrl = await AB.UploadBlobtest(file);
                db.EditWebshopItem(ws, imageUrl.ToString());

                return RedirectToAction("WebshopAdminList");
            }
        }
        //public ActionResult _DeleteWebshopItem(Webshop ws)
        //{
        //    return
        //}

        public ActionResult UploadImage()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> UploadImage(HttpPostedFileBase file)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                    }
                }
                var imageUrl = await AB.UploadBlobtest(file);
                TempData["LatestImage"] = imageUrl.ToString();

                return View();
            }
        }
    }
}