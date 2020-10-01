using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.Glue;
using Amazon.CDK.AWS.S3;
using CfnParameter = Amazon.CDK.CfnParameter;
using CfnParameterProps = Amazon.CDK.CfnParameterProps;
using CfnTable = Amazon.CDK.AWS.Glue.CfnTable;

namespace AthenaDemoProject
{
    public class AthenaDemoProjectStack : Stack
    {
        internal AthenaDemoProjectStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var database = new Database(this, "DemoFileDatabase", new DatabaseProps
            {
                DatabaseName = "demofiledb"
            });

            var athenaTable = new Amazon.CDK.AWS.Glue.Table(this, "ReceivedFileTable",
                new Amazon.CDK.AWS.Glue.TableProps
                {
                    Database = database,
                    Bucket = Bucket.FromBucketArn(this,"DataLakeBucket", "arn:aws:s3:::athena-demo-data-lake"),
                    TableName = "receivedfiledemo",
                    DataFormat = new DataFormat(new DataFormatProps
                    {
                        SerializationLibrary = new SerializationLibrary("org.apache.hadoop.hive.serde2.lazy.LazySimpleSerDe"),
                        OutputFormat = new OutputFormat("org.apache.hadoop.hive.ql.io.HiveIgnoreKeyTextOutputFormat"),
                        InputFormat = new InputFormat("org.apache.hadoop.mapred.TextInputFormat")
                    }),
                    PartitionKeys = new IColumn[]
                    {
                        new Column { Name = "year", Type = Schema.INTEGER },
                        new Column { Name = "month", Type = Schema.INTEGER }
                    },
                    Columns = new IColumn[]
                    {
                        new Column { Name = "id", Type = Schema.STRING },
                        new Column { Name = "filename", Type = Schema.STRING },
                        new Column { Name = "submissiondate", Type = Schema.STRING },
                        new Column { Name = "status", Type = Schema.STRING },
                       
                    },
                    Description = "demo receivedfiletalbe",
                    S3Prefix = "DataFiles/ReceivedFiles"
                });

            ((CfnTable)athenaTable.Node.DefaultChild)?.AddPropertyOverride("TableInput.StorageDescriptor.SerdeInfo.Parameters", new Dictionary<string, string>
            {
                ["field.delim"] = ",",
                ["skip.header.line.count"] = "1"
            });
        }
    }
}
