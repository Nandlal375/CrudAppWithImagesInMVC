using CrudAppWithImagesInMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrudAppWithImagesInMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        NEWDBEntities db = new NEWDBEntities();
        public ActionResult Index()
        {
            var data = db.Students.ToList();
            return View(data);
        }

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(Student s)
        {
            try
            {
                string filename = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
                string extension = Path.GetExtension(s.ImageFile.FileName);
                HttpPostedFileBase postedFile = s.ImageFile;
                int length = postedFile.ContentLength;
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    if (length <= 1000000)
                    {
                        filename = filename + extension;
                        s.image_path = "~/images/" + filename;
                        filename = Path.Combine(Server.MapPath("~/images/"), filename);
                        s.ImageFile.SaveAs(filename);
                        db.Students.Add(s);
                        int a = db.SaveChanges();
                        if (a > 0)
                        {
                            //ViewBag.Message = "<script>alert('Record Inserted !!')</script>";
                            TempData["Message"] = "<script>alert('Record Inserted !!')</script>";
                            ModelState.Clear();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Message"] = "<script>alert('Record Not Inserted !!')</script>";
                        }
                    }
                    else
                    {
                        TempData["Message"] = "<script>alert('Image not Should be max of 1 MB !!')</script>";

                    }
                }
                else
                {
                    TempData["Message"] = "<script>alert('Image Not Support !!')</script>";

                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

            }

            return View();
        }
        public ActionResult Edit(int id)
        {
           var row = db.Students.Where(model => model.id == id).FirstOrDefault();
            Session["image"] = row.image_path;
            return View(row);
        }
        [HttpPost]
        public ActionResult Edit(Student s)
        {
            if (ModelState.IsValid == true)
            {
                if (s.ImageFile != null)
                {
                    try
                    {
                        string filename = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
                        string extension = Path.GetExtension(s.ImageFile.FileName);
                        HttpPostedFileBase postedFile = s.ImageFile;
                        int length = postedFile.ContentLength;
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            if (length <= 1000000)
                            {
                                filename = filename + extension;
                                s.image_path = "~/images/" + filename;
                                filename = Path.Combine(Server.MapPath("~/images/"), filename);
                                s.ImageFile.SaveAs(filename);
                                //db.Students.Add(s);
                                db.Entry(s).State = EntityState.Modified;
                                int a = db.SaveChanges();
                                if (a > 0)
                                {
                                    TempData["updateMsg"] = "<script>alert('Record Updated !!')</script>";
                                    ModelState.Clear();
                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                {
                                    TempData["updateNotMsg"] = "<script>alert('Record Not Updated !!')</script>";
                                }
                            }
                            else
                            {
                                TempData["updateMsg"] = "<script>alert('Image not Should be max of 1 MB !!')</script>";

                            }
                        }
                        else
                        {
                            TempData["updateMsg"] = "<script>alert('Image Not Support !!')</script>";

                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = ex.Message;

                    }
                }
                else
                {
                    s.image_path = Session["image"].ToString();
                    db.Entry(s).State = EntityState.Modified;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["updateMsg"] = "<script>alert('Record Updated !!')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["updateNotMsg"] = "<script>alert('Record Not Updated !!')</script>";
                    }
                }
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                var row = db.Students.Where(model => model.id == id).FirstOrDefault();
                if (row != null)
                {
                    db.Entry(row).State = EntityState.Deleted;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["DeleteMessage"] = "<script>alert('Data Delete Succefully !!')</script>";
                        string ImagePath = Request.MapPath(row.image_path.ToString());
                        if (System.IO.File.Exists(ImagePath))
                        {
                            System.IO.File.Delete(ImagePath);

                        }
                    }
                    else
                    {
                        TempData["DeleteMessage"] = "<script>alert('Data Not Delete Succefully !!')</script>";
                    }
                }

            }
            return RedirectToAction("Index", "Home");  
        }

        public ActionResult Details(int id)
        {
            var data = db.Students.Where(model => model.id == id).FirstOrDefault();
            Session["Imgrow"] = data.image_path;
            return View(data);
        }
    }
}