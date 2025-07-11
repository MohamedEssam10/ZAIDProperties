using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.Property;
using ApplicationLayer.Helper;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using ApplicationLayer.Specifications.Properties;
using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class PropertyServices : IPropertyServices
    {
        public IUnitOfWork UnitOfWork { get; }
        public PropertyServices(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }


        public async Task<APIResponse<List<PropertyDTOResponse>>> GetAll(PropertyParams Params)
        {
            var spec = new GetAllPropertySpecs(Params);

            var propertyRepo = UnitOfWork.Repository<Property>();

            var query = propertyRepo.GetAllWithSpecification(spec);
            var properties = query.ToList();

            var propertyDTOs = properties.Select(p => new PropertyDTOResponse
            {
                Name = p.Name,
                Description = p.Description,
                Type = p.Type,
                Status = p.Status,
                Location = p.Location,
                Price = p.Price,
                Area = p.Area,
                // Assuming each Property entity has a List<Images> Images property
                Images = p.Images != null && p.Images.Any()
                        ? string.Join(",", p.Images.Select(i => URLResolver.BuildFileUrl(i.ImageUrl)))
                        : null
            }).ToList();


            return APIResponse<List<PropertyDTOResponse>>.SuccessResponse(
                200,
                propertyDTOs,
                "Properties retrieved successfully"
            );
        }

        public async Task<APIResponse<PropertyDTOResponse>> GetbyId(int Id)
        {
            var Specs = new GetPropertyById(Id);
            var propertyEntity = await UnitOfWork.Repository<Property>()
                .GetAllWithSpecification(Specs)
                .FirstOrDefaultAsync();

            if (propertyEntity is null)
            {
                return APIResponse<PropertyDTOResponse>.FailureResponse(
                    404,
                    new List<string> { "This Property Not Found" },
                    "Failed To Get Property"
                );
            }

         
            var dto = new PropertyDTOResponse
            {
                Id = propertyEntity.Id,
                Name = propertyEntity.Name,
                Description = propertyEntity.Description,
                Type = propertyEntity.Type,
                Status = propertyEntity.Status,
                Location = propertyEntity.Location,
                Price = propertyEntity.Price,
                Area = propertyEntity.Area,
                Images = URLResolver.BuildFileUrl(
                    propertyEntity.Images.FirstOrDefault(img => img.IsMainImage)?.ImageUrl
                    ?? propertyEntity.Images.FirstOrDefault()?.ImageUrl
                )
            };

            return APIResponse<PropertyDTOResponse>.SuccessResponse(200, dto, "Property Retrieved Successfully");



        }

        public async Task<APIResponse<PropertyDTOAdd>> AddProperty(PropertyDTOAdd propertyDTOAdd)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(propertyDTOAdd.Name))
                errors.Add("Property name is required.");
            if (string.IsNullOrEmpty(propertyDTOAdd.Description))
                errors.Add("Property description is required.");
            if (propertyDTOAdd.Price <= 0)
                errors.Add("Property price must be greater than zero.");

            if (errors.Count > 0)
                return APIResponse<PropertyDTOAdd>.FailureResponse(400, errors, "Validation failed.");

            var property = new Property
            {
                Name = propertyDTOAdd.Name,
                Description = propertyDTOAdd.Description,
                Type = propertyDTOAdd.Type,
                Status = propertyDTOAdd.Status,
                Location = propertyDTOAdd.Location,
                Price = propertyDTOAdd.Price,
                Area = propertyDTOAdd.Area,
                Images = new List<Images>
        {
            new Images
            {
                ImageUrl = URLResolver.BuildFileUrl(propertyDTOAdd.Images.FileName),
                IsMainImage = true
            }
        }
            };

            await UnitOfWork.Repository<Property>().AddAsync(property);
            var result = await UnitOfWork.CompleteAsync();

            if (!result) // or if (!result) depending on return type
            {
                return APIResponse<PropertyDTOAdd>.FailureResponse(
                    400,
                    new List<string> { "Failed to save the property to the database." },
                    "Failed To Add Property"
                );
            }

            return APIResponse<PropertyDTOAdd>.SuccessResponse(201, propertyDTOAdd, "Property Added Successfully");
        }

        public async Task<APIResponse<PropertyDTOUpdate>> UpdateProperty(PropertyDTOUpdate propertyDTOUpdate)
        {
            var errors = new List<string>();

          
            if (propertyDTOUpdate.Id <= 0)
                errors.Add("Invalid property Id.");
            if (string.IsNullOrEmpty(propertyDTOUpdate.Name))
                errors.Add("Property name is required.");
            if (string.IsNullOrEmpty(propertyDTOUpdate.Description))
                errors.Add("Property description is required.");
            if (propertyDTOUpdate.Price <= 0)
                errors.Add("Property price must be greater than zero.");

            if (errors.Count > 0)
                return APIResponse<PropertyDTOUpdate>.FailureResponse(400, errors, "Validation failed.");

            
            var existingProperty = await UnitOfWork.Repository<Property>()
                .GetByIdAsync(propertyDTOUpdate.Id);

            if (existingProperty == null)
            {
                return APIResponse<PropertyDTOUpdate>.FailureResponse(
                    404,
                    new List<string> { "Property not found." },
                    "Property Not Found"
                );
            }

       
            existingProperty.Name = propertyDTOUpdate.Name;
            existingProperty.Description = propertyDTOUpdate.Description;
            existingProperty.Type = propertyDTOUpdate.Type;
            existingProperty.Status = propertyDTOUpdate.Status;
            existingProperty.Location = propertyDTOUpdate.Location;
            existingProperty.Price = propertyDTOUpdate.Price;
            existingProperty.Area = propertyDTOUpdate.Area;

    
            if (propertyDTOUpdate.Images != null)
            {
                
                
                existingProperty.Images.Clear();
                existingProperty.Images.Add(new Images
                {
                    ImageUrl = URLResolver.BuildFileUrl(propertyDTOUpdate.Images.FileName),
                    IsMainImage = true
                });

            
            }

        
            UnitOfWork.Repository<Property>().Update(existingProperty);
            var result = await UnitOfWork.CompleteAsync();

            if (!result)
            {
                return APIResponse<PropertyDTOUpdate>.FailureResponse(
                    500,
                    new List<string> { "Failed to update the property in the database." },
                    "Failed To Update Property"
                );
            }

            return APIResponse<PropertyDTOUpdate>.SuccessResponse(201,
                propertyDTOUpdate,
                "Property updated successfully."
            );
        }

        public async Task<APIResponse<PropertyDTOResponse>> DeleteProperty(int id)
        {
            var errors = new List<string>();

            if (id <= 0)
                errors.Add("Invalid property ID.");

            if (errors.Count > 0)
                return APIResponse<PropertyDTOResponse>.FailureResponse(400, errors, "Validation failed.");

             var existingProperty = await UnitOfWork.Repository<Property>().GetByIdAsync(id);
            if (existingProperty == null)
            {
                return APIResponse<PropertyDTOResponse>.FailureResponse(
                    404,
                    new List<string> { "Property not found." },
                    "Delete Failed"
                );
            }
 
            UnitOfWork.Repository<Property>().Delete(existingProperty);
            var result = await UnitOfWork.CompleteAsync();

            if (!result )
            {
                return APIResponse<PropertyDTOResponse>.FailureResponse(
                    500,
                    new List<string> { "Failed to delete the property from the database." },
                    "Delete Failed"
                );
            }

            return APIResponse<PropertyDTOResponse>.SuccessResponse(
                200,
                null,
                "Property deleted successfully."
            );
        }

    }
}
 

 