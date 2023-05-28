using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Json.Serialization;
using Raven.Client.Json.Serialization.NewtonsoftJson;
using Sparrow;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{

    public class DeserializationSecurity
    {
        public DeserializationSecurity()
        {
        }

        void DeserializationSecurity_RegisterForbiddenNamespace()
        {
            object suspiciousObject = "suspiciousObject";
            object trustedObject = "trustedObject";

            #region DefaultRavenSerializationBinder
            // Create a default serialization binder
            var binder = new DefaultRavenSerializationBinder();
            // Register a forbidden namespace
            binder.RegisterForbiddenNamespace("SuspiciousNamespace");
            // Register a forbidden object type
            binder.RegisterForbiddenType(suspiciousObject.GetType());
            // Register a trusted object type
            binder.RegisterSafeType(trustedObject.GetType());

            var store = new DocumentStore()
            {
                Conventions =
                {
                    Serialization = new NewtonsoftJsonSerializationConventions
                    {
                        // Customize store deserialization using the defined binder
                        CustomizeJsonDeserializer = deserializer => deserializer.SerializationBinder = binder
                    }
                }
            };
            #endregion
        }


        void DeserializationSecurity_InvokeGadget1()
        {
            using (var store = new DocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    #region DeserializationSecurity_load-object
                    // The object will be allowed to be deserialized
                    // regardless of the default binder list.  
                    session.Load<object>("Gadget");
                    #endregion
                }
            }
        }

        void DeserializationSecurity_InvokeGadget()
        {
            #region DeserializationSecurity_define-type
            string userdata = @"{
                '$type':'System.Windows.Data.ObjectDataProvider, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35',
                'MethodName':'Start',
                'MethodParameters':{
                            '$type':'System.Collections.ArrayList, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089',
                    '$values':['cmd', '/c calc.exe']
                },
                'ObjectInstance':{'$type':'System.Diagnostics.Process, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'}
            }";
            #endregion
        }


        class foo
        {
            #region RegisterForbiddenNamespace_definition
            public void RegisterForbiddenNamespace(string @namespace)
            #endregion
            { }

            #region RegisterForbiddenType_definition
            public void RegisterForbiddenType(Type type)
            #endregion
            { }

            #region RegisterSafeType_definition
            public void RegisterSafeType(Type type)
            #endregion
            { }
        }
    }
}
