﻿using SmartShop.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartShop.Web.Areas.Admin.Models
{
    public class ProductModel
    {
        private ProductManagementService _productManagementService;
        private ProductCategoryManagementService _productCategoryManagementService;

        public string Name { get; set; }
        public double Price { get; set; }
        public virtual List<ProductCategory> ProductCategories { get; set; }
        public Guid ProductCategoryId { get; set; }
        public double SpecialPrice { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool IsNew { get; set; }
        public HttpPostedFileBase FeaturedImageFile { get; set; }

        public ProductModel()
        {
            _productManagementService = new ProductManagementService();
            _productCategoryManagementService = new ProductCategoryManagementService();
            ProductCategories = _productCategoryManagementService.GetAllCategories();
        }

        public void AddProduct(string name, double price, Guid productCategoryId, double specialPrice, int quantity, string description, bool isNew)
        {
            var imageUrl = UploadFeaturedImage(FeaturedImageFile);
            _productManagementService.AddProduct(name, price, productCategoryId, specialPrice, quantity, description, isNew);
        }

        private ProductImage UploadFeaturedImage(HttpPostedFileBase featuredImageFile)
        {
            if(featuredImageFile != null)
            {
                string uploadPath = "~/Temp";
                string originalName = featuredImageFile.FileName;
                string newName = Guid.NewGuid().ToString().Replace("-", "") + originalName.Substring(originalName.IndexOf('.'));
                string fullPath = uploadPath + "/" + newName;
                featuredImageFile.SaveAs(HttpContext.Current.Server.MapPath(fullPath));

                ProductImage image = new ProductImage();
                image.ImageUrl = fullPath;
                image.OriginalName = originalName;
                image.CurrentName = newName;

                return image;
            }
            return null;
        }

        public void DeleteProduct(Guid? id)
        {
            if (id.HasValue)
            {
                _productManagementService.DeleteProduct(id.Value);
            }
            else
            {
                throw new Exception();
            }
                
        }
       
    }
}