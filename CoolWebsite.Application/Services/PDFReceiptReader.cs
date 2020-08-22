using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.Services.Models;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;

namespace CoolWebsite.Application.Services
{
    public class PdfReceiptReader
    {
        private enum State
        {
            Stop,
            Start
        }
        
        private readonly IFormFile _pdfReceipt;
        private readonly List<PdfReceiptItemDto> _pdfReceiptItems;
        private string _location = String.Empty;
        private DateTime _date;
        
        public PdfReceiptReader(IFormFile file)
        {
            _pdfReceipt = file;
            _pdfReceiptItems = new List<PdfReceiptItemDto>();
        }

        public PdfReceiptDto GetReceipt()
        {
            var receipt = new PdfReceiptDto();

            ExtractData();

            receipt.DateVisited = _date;
            receipt.Location = _location;
            receipt.PdfReceiptItems = _pdfReceiptItems;

            return receipt;
        }

        private void ExtractData()
        {
            
            var state = State.Stop;
            var reader = new PdfReader(_pdfReceipt.OpenReadStream());
            var streamBytes = reader.GetPageContent(1);
            var tokenizer = new PrTokeniser(new RandomAccessFileOrArray(streamBytes));
            
            int num = 1;
            var pdfReceiptItem = new PdfReceiptItemDto();

            bool isFirstWordTaken = false;
            while (tokenizer.NextToken())
            {
                if (tokenizer.TokenType == PrTokeniser.TK_STRING)
                {
                    var currentText = tokenizer.StringValue;
                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default,
                        Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    Console.WriteLine(currentText);

                    //GetLocation
                    if (!isFirstWordTaken && !string.IsNullOrWhiteSpace(currentText))
                    {
                        _location = currentText;
                        isFirstWordTaken = true;
                    }
                    
                    //GetDate
                    if (currentText.Contains("Dato:")) {
                        var formatted = GetFormattedDateString(currentText.Remove(0, 6));
                        _date = DateTime.Parse(formatted);
                    }

                    //GetReceiptItem
                    if (state == State.Start)
                    {
                        //navn 1 -> antal  2 -> pris 3 -> reset
                        switch (num)
                        {
                            case 1:
                                //name
                                pdfReceiptItem.Name = currentText;
                                if (currentText == "Rabat")
                                {
                                    pdfReceiptItem.IsDiscount = true;
                                }
                                num++;
                                break;
                            case 2:
                                //antal
                                pdfReceiptItem.Count = (int)double.Parse(currentText);
                                num++;
                                break;
                            case 3:
                                pdfReceiptItem.Price = double.Parse(currentText);

                                _pdfReceiptItems.Add(pdfReceiptItem);

                                pdfReceiptItem = new PdfReceiptItemDto();

                                num = 1;
                                break;
                        }
                    }

                    //setState
                    if (currentText == "Pris")
                    {
                        state = State.Start;
                    }
                    
                    //end
                    else if (currentText == "I alt inkl. moms")
                    {
                        break;
                    }
                }
            }
        }
        
        private static string GetFormattedDateString(string date)
        {
            var month = GetMonth(date);
            var monthNumber = ConvertMonthToNumber(month);
            if (monthNumber == -1) throw new Exception("meeh");
            
            var formatted = date.Replace("."," ").Replace(month, $"/{monthNumber}/");
            return formatted;
        }

        private static int ConvertMonthToNumber(string month)
        {
            switch (month.ToLower())
            {
                case "januar":
                    return 1;
                case "februar":
                    return 2;
                case "marts":
                    return 3;
                case "april":
                    return 4;
                case "maj":
                    return 5;
                case "juni":
                    return 6;
                case "juli":
                    return 7;
                case "august":
                    return 8;
                case "september":
                    return 9;
                case "oktober":
                    return 10;
                case "november":
                    return 11;
                case "december":
                    return 12;
            }

            return -1;
        }

        private static string GetMonth(string date)
        {
            var month = new StringBuilder();
            
            foreach (var c in date.ToCharArray())
            {
                if (Char.IsLetter(c))
                {
                    month.Append(c);
                }
            }

            return month.ToString();
        }
    }
}