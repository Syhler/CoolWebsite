using System.Collections.Generic;
using System.Linq;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries.Models;
using CoolWebsite.Domain.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.Financial.Common
{
    public static class SelectListHandler
    {

        public static List<SelectListItem> CreateFromUsers(IList<ApplicationUser> models, string userId)
        {
            var listOfSelectedItem = new List<SelectListItem>();

            foreach (var user in models)
            {
                if (user.Id == userId)
                {
                    continue;
                }
                
                listOfSelectedItem.Add(new SelectListItem
                {
                    Value = user.Id,
                    Text = user.FirstName +" " + user.LastName
                });
            }

            return listOfSelectedItem;
        }
        
        public static List<SelectListItem> CreateFromUsers(IList<UserDto> models)
        {
            var listOfSelectedItem = new List<SelectListItem>();

            foreach (var user in models)
            {
                listOfSelectedItem.Add(new SelectListItem
                {
                    Value = user.Id,
                    Text = user.Name
                });
            }

            return listOfSelectedItem;
        }

        public static List<SelectListItem> CreateFromItemGroup(IList<ItemGroupDto> itemGroups)
        {
            var listOfSelectedItem = new List<SelectListItem>();

            foreach (var itemGroup in itemGroups)
            {
                listOfSelectedItem.Add(new SelectListItem
                {
                    Value = itemGroup.Value.ToString(),
                    Text = itemGroup.Name
                });
            }

            return listOfSelectedItem;
        }
    }
}