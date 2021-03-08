///
///    Experimented By : Ozesh Thapa
///    Email: dablackscarlet@gmail.com
///
using System;
using System.Collections.Generic;
using System.Text;
using BioMetrixCore.DB;
using MongoDB.Bson;
using MongoDB.Driver;


namespace BioMetrixCore
{
    internal class DeviceManipulator
    {


        public ICollection<MachineInfo> GetLogData(ZkemClient objZkeeper, int machineNumber)
        {
            string dwEnrollNumber1 = "";
            int dwVerifyMode = 0;
            int dwInOutMode = 0;
            int dwYear = 0;
            int dwMonth = 0;
            int dwDay = 0;
            int dwHour = 0;
            int dwMinute = 0;
            int dwSecond = 0;
            int dwWorkCode = 0;

            ICollection<MachineInfo> lstEnrollData = new List<MachineInfo>();

            objZkeeper.ReadAllGLogData(machineNumber);
          

            while (objZkeeper.SSR_GetGeneralLogData(machineNumber, out dwEnrollNumber1, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))


            {
                string inputDate = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond).ToString();

                MachineInfo objInfo = new MachineInfo();
                objInfo.MachineNumber = machineNumber;
                objInfo.IndRegID = int.Parse(dwEnrollNumber1);
                objInfo.DateTimeRecord = inputDate;
                objInfo.dwInOutMode = dwInOutMode;

                lstEnrollData.Add(objInfo);

                Console.WriteLine("dwInOutMode -" + objInfo.dwInOutMode);

                if (objInfo.dwInOutMode == 0)
                { 
                    push_mongo.checkin(objInfo.IndRegID, objInfo.DateTimeRecord);
                }
                else if (objInfo.dwInOutMode == 1)
                {
                    push_mongo.checkout(objInfo.IndRegID, objInfo.DateTimeRecord);
                }
                else if (objInfo.dwInOutMode == 2)
                {
                    push_mongo.otin(objInfo.IndRegID, objInfo.DateTimeRecord,"ot");
                }
                else if (objInfo.dwInOutMode == 4)
                {
                    push_mongo.otout(objInfo.IndRegID, objInfo.DateTimeRecord,"ot");

                }
                else if (objInfo.dwInOutMode == 5)
                {
                    push_mongo.otin(objInfo.IndRegID, objInfo.DateTimeRecord, "break");
                }
                else if (objInfo.dwInOutMode == 6)
                {
                    push_mongo.otout(objInfo.IndRegID, objInfo.DateTimeRecord, "otout");

                }
            }
            objZkeeper.ClearGLog(machineNumber);
            return lstEnrollData;
        }




  

        public string FetchDeviceInfo(ZkemClient objZkeeper, int machineNumber)
        {
            StringBuilder sb = new StringBuilder();

            string returnValue = string.Empty;


            objZkeeper.GetFirmwareVersion(machineNumber, ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("Firmware V: ");
                sb.Append(returnValue);
                sb.Append(",");
            }


            returnValue = string.Empty;
            objZkeeper.GetVendor(ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("Vendor: ");
                sb.Append(returnValue);
                sb.Append(",");
            }

            string sWiegandFmt = string.Empty;
            objZkeeper.GetWiegandFmt(machineNumber, ref sWiegandFmt);

            returnValue = string.Empty;
            objZkeeper.GetSDKVersion(ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("SDK V: ");
                sb.Append(returnValue);
                sb.Append(",");
            }

            returnValue = string.Empty;
            objZkeeper.GetSerialNumber(machineNumber, out returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("Serial No: ");
                sb.Append(returnValue);
                sb.Append(",");
            }

            returnValue = string.Empty;
            objZkeeper.GetDeviceMAC(machineNumber, ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("Device MAC: ");
                sb.Append(returnValue);
            }

            return sb.ToString();
        }



    }
}
