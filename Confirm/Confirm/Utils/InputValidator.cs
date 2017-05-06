using Confirm.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Confirm.Utils
{
    public class InputValidator
    {
        public InputValidator()
        {

        }

        //public string ValidateConfirmRecord(ConfirmRecord confirmRecord)
        //{
        //    if (confirmRecord.ImageName == "")
        //        return "Please capture an Image";
        //    else if (confirmRecord.ActiveCampaign == "")
        //        return "Please select a Campaign";
        //    else if (confirmRecord.Brand1 == "" && confirmRecord.Brand2 == "" && confirmRecord.Brand3 == "")
        //        return "Please select atleast one Brand";
        //    else if (confirmRecord.StoreInformation == null)
        //        return "Please enter Store information";
        //    else
        //        return "valid";
        //}

        public List<bool> ValidationResult(ConfirmRecord confirmRecord, out bool IsValid)
        {
            List<bool> validationResult = new List<bool>(3) { false, false, false };

            bool[] validateResult = { false,false,false }; 
            

            Debug.WriteLine("INDEX OF ARRAY : " + validationResult.Count);
            if (!(confirmRecord.ImageName == ""))
            {
                validationResult[0] = true;
                Debug.WriteLine("Imagename : " + confirmRecord.ImageName);
                Debug.WriteLine("Imagename : " + true);
            }

            if (!(confirmRecord.Brand1 == "" && confirmRecord.Brand2 == "" && confirmRecord.Brand3 == ""))
            {
                validationResult[1] = true;
                Debug.WriteLine("Brands : " + true);
            }

            if (!(confirmRecord.StoreInformation == null) && !(confirmRecord.StoreInformation == ""))
            {
                validationResult[2] = true;
                Debug.WriteLine("Store Information : " + true);
                Debug.WriteLine("Store Information : " + confirmRecord.StoreInformation);
            }


            IsValid = IsValidConfirmRecord(validationResult);

            Debug.WriteLine("Result : " + IsValid);

            return validationResult;
        }

        private bool IsValidConfirmRecord(List<bool> validationResults)
        {
            bool IsValidRecord = true;
            foreach (bool singleResult in validationResults)
            {
                if (!singleResult)
                    IsValidRecord = false;
            }
            return IsValidRecord;
        }
    }
}
