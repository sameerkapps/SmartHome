using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;

using HomeHub.Model;

using SmartHomeRESTAPI.TableEntities;

namespace SmartHomeRESTAPI.AzureStorageDB
{
    /// <summary>
    /// Repository for the sense hat
    /// </summary>
    public class SensorHatRepository : ISensorHatRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SensorHatRepository()
        {
            _table = CreateCloudTableClient();
        }

        /// <summary>
        /// Add sensor data
        /// </summary>
        /// <param name="deviceId">Device Id</param>
        /// <param name="temperature">Temperature</param>
        /// <param name="humidity">Humidity</param>
        /// <param name="localTime">Local time</param>
        /// <returns></returns>
        public bool AddSensorHatData(string deviceId, int temperature, int humidity, DateTime localTime)
        {
            long ticks = DateTime.UtcNow.Ticks;
            var latestEnt = new SensorHatEntity(deviceId, temperature, humidity, localTime, true, ticks);
            var oldestEnt = new SensorHatEntity(deviceId, temperature, humidity, localTime, false, ticks);

            // EntityGroupTransaction
            TableBatchOperation batch = new TableBatchOperation();
            batch.InsertOrMerge(latestEnt);
            batch.InsertOrMerge(oldestEnt);

            try
            {
                _table.ExecuteBatch(batch);

                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);

                return false;
            }
        }

        /// <summary>
        /// Gets the data as per the sorted order
        /// </summary>
        /// <param name="deviceId">Id of the device</param>
        /// <param name="startUTCTime">Start time in UTC</param>
        /// <param name="endUTCTime">End time in UTC</param>
        /// <param name="latestFirst">Order of the data</param>
        /// <returns></returns>
        public List<SensorHatModel> GetSensorHatData(string deviceId, DateTime startUTCTime, DateTime endUTCTime, bool latestFirst)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new ArgumentNullException(nameof(deviceId));
            }

            // ensure that the params are valid
            if (startUTCTime < endUTCTime)
            {
                throw new ArgumentException("Start time should be less than the end time.");
            }

            // return list
            List<SensorHatModel> data = new List<SensorHatModel>();
            try
            {
                // get the start and end row keys
                var startRowKey = SensorHatEntity.GenerateRowKey(latestFirst, startUTCTime.Ticks);
                var endRowKey = SensorHatEntity.GenerateRowKey(latestFirst, endUTCTime.Ticks);

                // build the filter for PartitionKey
                var pkFilter = TableQuery.GenerateFilterCondition(nameof(TableEntity.PartitionKey), QueryComparisons.Equal, deviceId);

                // filter fr ow keys
                var gtEqRowFilter = TableQuery.GenerateFilterCondition(nameof(TableEntity.RowKey), QueryComparisons.GreaterThanOrEqual, startRowKey);
                var ltEqRowFilter = TableQuery.GenerateFilterCondition(nameof(TableEntity.RowKey), QueryComparisons.LessThanOrEqual, endRowKey);

                // combine the filters
                var rowRangeFilter = TableQuery.CombineFilters(gtEqRowFilter, TableOperators.And, ltEqRowFilter);
                var filter = TableQuery.CombineFilters(pkFilter, TableOperators.And, rowRangeFilter);

                // run the queries
                var tableQuery = new TableQuery<SensorHatEntity>().Where(filter);

                var results = _table.ExecuteQuery(tableQuery);
                //convert entities into models
                foreach (var ent in results)
                {
                    var model = new SensorHatModel();
                    model.DeviceId = ent.PartitionKey;
                    model.Humidity = ent.Humidity;
                    model.Temperature = ent.Temperature;
                    model.TimeStamp = ent.LocalTime;

                    data.Add(model);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }

            return data;
        }

        #region Table 
        /// <summary>
        /// creates client
        /// </summary>
        /// <returns></returns>
        private static CloudTable CreateCloudTableClient()
        {
            var connString = ConfigurationManager.AppSettings[ConnectionStringKey];
            var storageClient = CloudStorageAccount.Parse(connString);
            return storageClient.CreateCloudTableClient().GetTableReference(TableName);
        }

        /// <summary>
        /// Create table, if it does not exist
        /// </summary>
        /// <returns></returns>
        internal static bool EnsureCreated()
        {
            try
            {
                var table = CreateCloudTableClient();
                table.CreateIfNotExists();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        // table
        CloudTable _table;


        internal const string TableName = "WeatherData";
        internal const string ConnectionStringKey = "AzureStorageConnectionKey";
        #endregion
    }
}