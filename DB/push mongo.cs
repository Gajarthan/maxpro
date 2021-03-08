using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;

namespace BioMetrixCore.DB
{
    class push_mongo
    {
        protected static string id = "007";
        public static MongoClient connect()
        {
            MongoClient dbClient = new MongoClient("mongodb+srv://abilash:abilash@hris.popyq.mongodb.net/WeeBreakkHCI?authSource=admin&replicaSet=atlas-coyxb9-shard-0&w=majority&readPreference=primary&appname=MongoDB%20Compass&retryWrites=true&ssl=true");
          

            return dbClient;
        }

        public static bool checkin(int indexNo, string DateTimeRecord)
        {
            var dbClient = connect();
            var database = dbClient.GetDatabase("WeeBreakkHCI");
            var collection = database.GetCollection<BsonDocument>(id);
            var filter1 = Builders<BsonDocument>.Filter.Eq("Type", "Employee-info");
            var filter2 = Builders<BsonDocument>.Filter.Eq("IndexNo", indexNo.ToString("D4"));
            var documents = collection.Find(filter1 & filter2).ToList();

            foreach (BsonDocument doc in documents)
            {
                var _id = @doc.GetElement("_id").Value;
                var inTime = @doc.GetElement("inTime").Value;
                var outTime = @doc.GetElement("outTime").Value;
                var document = new BsonDocument { { "date", changeTime(DateTime.Parse(DateTimeRecord),5,30,00,00) },
                    { "EmpID", _id.ToString() },
                    { "ShiftIn", inTime },
                    { "ShiftOut", outTime },
                    { "ActualIn", DateTime.Parse(DateTimeRecord) },
                    { "ActualOut", "" },
                    { "OutDate" ,""},
                    { "Type", "attendance" },
                    { "CreatedAt", DateTime.Now }
                };
                collection.InsertOne(document);

            }
            return true;
        }

        public static bool checkout(int indexNo, string DateTimeRecord)
        {
            var dbClient = connect();
            var database = dbClient.GetDatabase("WeeBreakkHCI");
            var collection = database.GetCollection<BsonDocument>(id);

            var filter1 = Builders<BsonDocument>.Filter.Eq("Type", "Employee-info");
            var filter2 = Builders<BsonDocument>.Filter.Eq("IndexNo", indexNo.ToString("D4"));
            var documents = collection.Find(filter1 & filter2).ToList();


            foreach (BsonDocument doc in documents)
            {
                var _id = @doc.GetElement("_id").Value;
                var filter = Builders<BsonDocument>.Filter.Eq("EmpID", _id.ToString()) &
                             Builders<BsonDocument>.Filter.Eq("Type", "attendance") &
                             Builders<BsonDocument>.Filter.Eq("ActualOut", "");


                var documentsx = collection.Find<BsonDocument>(filter).ToList();
                Console.WriteLine(documentsx.Count);
                foreach (BsonDocument doc1 in documentsx)
                {
                    var _idn = @doc1.GetElement("_id").Value;

                    if (documentsx.Count <= 0)
                    {

                        var inTime = @doc.GetElement("inTime").Value;
                        var outTime = @doc.GetElement("outTime").Value;
                        var document = new BsonDocument
                    {
                        {"date", changeTime(DateTime.Parse(DateTimeRecord), 5, 30, 00, 00)},
                        {"EmpID", _id.ToString()},
                        {"ShiftIn", inTime},
                        {"ShiftOut", outTime},
                        {"ActualIn", ""},
                        {"ActualOut", DateTime.Parse(DateTimeRecord)},
                        {"OutDate", changeTime(DateTime.Parse(DateTimeRecord), 5, 30, 00, 00)},
                        {"Type", "attendance"},
                        {"CreatedAt", DateTime.Now}
                    };
                        collection.InsertOne(document);
                    }
                    else
                    {
                        var filterx = Builders<BsonDocument>.Filter.Eq("_id", _idn);
                        var update = Builders<BsonDocument>.Update.Set("ActualOut", DateTime.Parse(DateTimeRecord));
                        var update1 = Builders<BsonDocument>.Update.Set("OutDate",
                            changeTime(DateTime.Parse(DateTimeRecord), 5, 30, 00, 00));
                        var ActualIn = @doc1.GetElement("ActualIn").Value;
                        TimeSpan ts = DateTime.Parse(DateTimeRecord) - (DateTime)ActualIn;
                        Console.WriteLine(ts.TotalMinutes);
                        var document = collection.Find(filterx).ToList();
                        BsonDocument doc2 = document.LastOrDefault();
                        if (ts.TotalMinutes <= 1440)
                        {
                            collection.UpdateOne(filterx, update1);
                            collection.UpdateOne(filterx, update);
                        }
                        else
                        {
                            var inTime = @doc.GetElement("inTime").Value;
                            var outTime = @doc.GetElement("outTime").Value;
                            var documenta = new BsonDocument
                        {
                            {"date", changeTime(DateTime.Parse(DateTimeRecord), 5, 30, 00, 00)},
                            {"EmpID", _id.ToString()},
                            {"ShiftIn", inTime},
                            {"ShiftOut", outTime},
                            {"ActualIn", ""},
                            {"ActualOut", DateTime.Parse(DateTimeRecord)},
                            {"OutDate", changeTime(DateTime.Parse(DateTimeRecord), 5, 30, 00, 00)},
                            {"Type", "attendance"},
                            {"CreatedAt", DateTime.Now}
                        };
                            collection.InsertOne(documenta);
                        }
                    }
                }

                    
            }
            return true;
        }
        public static bool otin(int indexNo, string DateTimeRecord , string type)
        {

            var dbClient = connect();
            var database = dbClient.GetDatabase("WeeBreakkHCI");
            var collection = database.GetCollection<BsonDocument>(id);

            //Get User Info
            var filter1 = Builders<BsonDocument>.Filter.Eq("Type", "Employee-info");
            var filter2 = Builders<BsonDocument>.Filter.Eq("IndexNo", indexNo.ToString("D4"));
            var documents = collection.Find(filter1 & filter2).ToList();
            BsonDocument doc = documents.FirstOrDefault();
            var _id = @doc.GetElement("_id").Value;
            //Attandace info
            var filter = Builders<BsonDocument>.Filter.Eq("EmpID", _id.ToString()) &
                         Builders<BsonDocument>.Filter.Eq("Type", "attendance") &
                         Builders<BsonDocument>.Filter.Eq("ActualOut", "");

            var documents2 = collection.Find(filter).ToList();
            if (documents2.Count == 0)
            {
                var inTime = @doc.GetElement("inTime").Value;
                var outTime = @doc.GetElement("outTime").Value;
                var documents3 = new BsonDocument { { "date", changeTime(DateTime.Parse(DateTimeRecord),5,30,00,00) },
                    { "EmpID", _id.ToString() },
                    { "ShiftIn", inTime },
                    { "ShiftOut", outTime },
                    { "ActualIn", DateTime.Parse(DateTimeRecord) },
                    { "ActualOut", "" },
                    {"OutDate" ,""},
                    { "Type", "attendance" },
                    { "CreatedAt", DateTime.Now }
                };
                collection.InsertOne(documents3);
                var documents4 = new BsonDocument { { "date",
                        changeTime(DateTime.Parse(DateTimeRecord), 5, 30, 00, 00) },
                        { "EmpID", _id.ToString() },
                        { "StartTime", DateTime.Parse(DateTimeRecord) },
                        { "EndTime", ""},
                        { "Type", type },
                        { "EndDate", "" },
                        { "TimeSpent", 0 },
                        { "CreatedAt", DateTime.Now }
                    };
                collection.InsertOne(documents4);
            }
            else
            {
                var documents5 = new BsonDocument { { "date",
                        changeTime(DateTime.Parse(DateTimeRecord), 5, 30, 00, 00) },
                        { "EmpID", _id.ToString() },
                        { "StartTime", DateTime.Parse(DateTimeRecord) },
                        { "EndTime", ""},
                        { "Type", type },
                        { "EndDate", "" },
                        { "TimeSpent", 0 },
                        { "CreatedAt", DateTime.Now }
                    };
                collection.InsertOne(documents5);
            }
            return true;

        }
        public static bool otout(int indexNo, string DateTimeRecord, string type)
        {

            var dbClient = connect();
            var database = dbClient.GetDatabase("WeeBreakkHCI");
            var collection = database.GetCollection<BsonDocument>(id);

            //Get User Info
            var filter1 = Builders<BsonDocument>.Filter.Eq("Type", "Employee-info");
            var filter2 = Builders<BsonDocument>.Filter.Eq("IndexNo", indexNo.ToString("D4"));
            var documents = collection.Find(filter1 & filter2).ToList();
            BsonDocument doc = documents.FirstOrDefault();
            var _id = @doc.GetElement("_id").Value;
            //Attandace info
            if (documents.Count!=0)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("EmpID", _id.ToString()) &
                             Builders<BsonDocument>.Filter.Eq("Type", "ot") &
                             Builders<BsonDocument>.Filter.Eq("EndTime", "");

                var documents2 = collection.Find<BsonDocument>(filter).ToList();
                BsonDocument doc2 = documents2.LastOrDefault();
                var _idn = @doc2.GetElement("_id").Value;

                var filter3 = Builders<BsonDocument>.Filter.Eq("_id", _idn);
                var update = Builders<BsonDocument>.Update.Set("EndTime", DateTime.Parse(DateTimeRecord));
                var update1 = Builders<BsonDocument>.Update.Set("EndDate",
                    changeTime(DateTime.Parse(DateTimeRecord), 5, 30, 00, 00));

                var StartTime = @doc2.GetElement("StartTime").Value;
                var StartTimex = (DateTime)StartTime;
                TimeSpan ts = changeTime(DateTime.Parse(DateTimeRecord), DateTime.Parse(DateTimeRecord).Hour, DateTime.Parse(DateTimeRecord).Minute, 0, 0) - changeTime(StartTimex, StartTimex.Hour, StartTimex.Minute, 0, 0);
                Console.WriteLine(DateTime.Parse(DateTimeRecord));
                Console.WriteLine((DateTime)StartTime);
                Console.WriteLine(ts.TotalSeconds);
                var update2 = Builders<BsonDocument>.Update.Set("TimeSpent", (int)ts.TotalMilliseconds- 1.98e+7);

                if (ts.TotalMinutes <= 1440)
                {
                    checkout(indexNo, DateTimeRecord);
                    collection.UpdateOne(filter3, update1);
                    collection.UpdateOne(filter3, update2);
                    collection.UpdateOne(filter3, update);
                }
            }
            
            return true;

        }
         public static bool breakin(int indexNo, string DateTimeRecord)
        {
            var dbClient = connect();
            var database = dbClient.GetDatabase("WeeBreakkHCI");
            var collection = database.GetCollection<BsonDocument>(id);

            //Get User Info
            var filter1 = Builders<BsonDocument>.Filter.Eq("Type", "Employee-info");
            var filter2 = Builders<BsonDocument>.Filter.Eq("IndexNo", indexNo.ToString("D4"));
            var documents = collection.Find(filter1 & filter2).ToList();
            BsonDocument doc = documents.FirstOrDefault();
            var _id = @doc.GetElement("_id").Value;

            //Attandace info
            var filter = Builders<BsonDocument>.Filter.Eq("EmpID", _id.ToString()) &
                         Builders<BsonDocument>.Filter.Eq("Type", "attendance") &
                         Builders<BsonDocument>.Filter.Eq("ActualOut", "");

            var documents2 = collection.Find(filter).ToList();
            if (documents2.Count == 0)
            {
                var documents4 = new BsonDocument { { "date",
                        changeTime(DateTime.Parse(DateTimeRecord), 5, 30, 00, 00) },
                        { "EmpID", _id.ToString() },
                        { "StartTime", DateTime.Parse(DateTimeRecord) },
                        { "EndTime", ""},
                        { "EndDate","" },
                        { "TimeSpent",0 },
                        { "Type", "break" },
                        { "CreatedAt", DateTime.Now }
                    };
                collection.InsertOne(documents4);
            }
            return true;

        }
        public static bool breakout(int indexNo, string DateTimeRecord)
        {
            var dbClient = connect();
            var database = dbClient.GetDatabase("WeeBreakkHCI");
            var collection = database.GetCollection<BsonDocument>(id);

            //Get User Info
            var filter1 = Builders<BsonDocument>.Filter.Eq("Type", "Employee-info");
            var filter2 = Builders<BsonDocument>.Filter.Eq("IndexNo", indexNo.ToString("D4"));
            var documents = collection.Find(filter1 & filter2).ToList();
            BsonDocument doc = documents.FirstOrDefault();
            var _id = @doc.GetElement("_id").Value;

            //Attandace info
            if (documents.Count!=0)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("EmpID", _id.ToString()) &
                             Builders<BsonDocument>.Filter.Eq("Type", "break") &
                             Builders<BsonDocument>.Filter.Eq("EndTime", "");

                var documents2 = collection.Find<BsonDocument>(filter).ToList();
                BsonDocument doc2 = documents2.LastOrDefault();
                if (documents2.Count != 0)
                {
                    var _idn = @doc2.GetElement("_id").Value;

                    var filter3 = Builders<BsonDocument>.Filter.Eq("_id", _idn);
                    var update = Builders<BsonDocument>.Update.Set("EndTime", DateTime.Parse(DateTimeRecord));
                    checkout(indexNo, DateTimeRecord);
                    collection.UpdateOne(filter3, update);
                }
            }
            return true;
        }

        public static DateTime changeTime(DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
            return new DateTime(
                    dateTime.Year,
                    dateTime.Month,
                    dateTime.Day,
                    hours,
                    minutes,
                    seconds,
                    milliseconds,
                    dateTime.Kind);
        }
    }
}
