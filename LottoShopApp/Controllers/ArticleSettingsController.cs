﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceDbContext;
using ECommerceDbContext.ECOMDBENTITIES;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using ShopModels.ViewModel;

namespace E_CommerceApp.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class ArticleSettingsController : ControllerBase
    {

        private readonly ECOMDBContext _db;

        private IArticleDetailsPostService _services;
        private IPostArticleImageManager _postArticleImageManager;
        private ICourierMasterPostManager _courierMasterPostManager;
        private IOrderDetailsTempManager _orderDetailsTempManager;
        private readonly IMapper _mapper;
        private readonly IPostCourierAgreementManager _postCourierAgreementManager;

        public ArticleSettingsController(ECOMDBContext db, 
            IArticleDetailsPostService services, 
            IPostArticleImageManager postArticleImageManager, 
            ICourierMasterPostManager courierMasterPostManager,
           IOrderDetailsTempManager orderDetailsTempManager,
            IMapper mapper,
            IPostCourierAgreementManager postCourierAgreementManager)
        {
            _db = db;
            _services = services;
            _postArticleImageManager = postArticleImageManager;
            _courierMasterPostManager = courierMasterPostManager;
            _orderDetailsTempManager = orderDetailsTempManager;
             _mapper = mapper;
            _postCourierAgreementManager = postCourierAgreementManager;
        }
       /* [HttpGet]
        public IActionResult GetCategorys()
        {
            return Ok(_db.Categorys);
        }
        [HttpGet]
        public IActionResult GetSubCategorys()
        {
            return Ok(_db.SubCategories);
        }

        [HttpGet]
        public IActionResult GetBrand()
        {
            return Ok(_db.Brands);
        }
        [HttpGet]
        public IActionResult GetVat()
        {
            return Ok(_db.Vats);
        }*/

        [HttpPost]
        public IActionResult GetAllArticleData(ArticleDetailsDto model)
        {
            var result = _services.ArticleDetailsPost(model);
            if (result != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult PostArticleImage(List<ArticleImageVm> model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;


                var user = _db.Users.FirstOrDefault(c => c.UserName == userId);
               
                foreach(var item in model)
                {
                    item.userId = user.LoginId;
                }

            }
            var result = _postArticleImageManager.PostEArticleImage(model);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult PostCourierMaster(CourierMasterVm model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var masterData = model.CourierDetails;
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;


                var user = _db.Users.FirstOrDefault(c => c.UserName == userId);

                foreach (var item in model.ContactPersonList)
                {
                    item.created_By = user.LoginId;
                    masterData.createdBy = user.LoginId;
                }

            }
            var res = _courierMasterPostManager.CourierMasterPost(model);
            return Ok(res);

        }

        [HttpPost]
        public IActionResult PostAgreement(CourierAgreementVm model)
        {
            var res = _postCourierAgreementManager.PostCourierAgreement(model);
            return Ok(res);
        }

        [HttpPost]
        public IActionResult PostOrderDetails(OrderDetailsTmpVm model)
        {
            var res = _orderDetailsTempManager.PostOrderDetailsTmp(model);

            return Ok(res);
        }
    }
}
