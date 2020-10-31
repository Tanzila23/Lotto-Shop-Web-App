﻿using AutoMapper;
using CommonUnitOfWork;
using Services.Contracts;
using ShopModels.Models;
using ShopModels.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Implementation
{
    public class ArticleDetailsPostService: IArticleDetailsPostService
    {
        private IUnitOfWork _unitOfWork;
       
        private readonly IMapper _mapper;
        public ArticleDetailsPostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public string ArticleDetailsPost(ArticleDetailsDto model)
        {
            var dataArticleDetails = model.ArticleDetailsData;
            Pricing price = new Pricing();
            string res = " ";
            try
            {

                var dataArt = _mapper.Map<ArticleDetails>(dataArticleDetails);
                var dataPr = _mapper.Map<Pricing>(dataArticleDetails);
                if (dataArt.ArtD_Id == 0)
                {
                   foreach(var Image in model.ArticleImageList)
                    {
                        string finalImage = "";
                        int pos = 12;
                        if (pos != 0)
                        {
                            // String after founder  
                            string afterFounder = Image.ImageName.Remove(pos);

                            // Remove everything before founder but include founder  
                            string beforeFounder = Image.ImageName.Remove(0, pos);
                            finalImage = beforeFounder;

                        }
                        
                        Image.ImageName = finalImage;
                        dataArt.ArticleImageVarients.Add(Image);
                    }
                   foreach(var variant in model.ArticleVariantList)
                    {
                       
                        dataArt.ArticleVariants.Add(variant);
                    }
                    _unitOfWork.ArticleDetails.Add(dataArt);
                    _unitOfWork.Commit();
                }
                else
                {
                    return "Likhon";
                }
                if (dataPr.Pricing_Id == 0)
                {
                    dataPr.ArticleDetails_Id = dataArt.ArtD_Id;
                    _unitOfWork.Pricing.Add(dataPr);
                }


            }
            catch (Exception ex)
            {

                res = ex.Message;
            }
            _unitOfWork.Commit();
            return res;
        }
    }
}
