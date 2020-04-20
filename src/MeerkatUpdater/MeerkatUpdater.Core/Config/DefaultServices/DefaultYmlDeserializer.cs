using Newtonsoft.Json;
using System;
using YamlDotNet.Serialization;

namespace MeerkatUpdater.Core.Config.DefaultServices
{
    /// <summary>
    /// The default deserializer for Yaml to grant that have a safe way to transform <br/>
    /// the file into a class
    /// </summary>
    public static class DefaultYmlDeserializer
    {
        private static readonly ISerializer JsonSerialization = new SerializerBuilder().JsonCompatible().Build();

        private static readonly IDeserializer DefaultDeserializer = new DeserializerBuilder().Build();

        /// <summary>
        /// Deserialize a Yaml payload into a target class
        /// </summary>
        /// <typeparam name="TTargetClass"></typeparam>
        /// <param name="payload"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static TTargetClass? Deserialize<TTargetClass>(string payload) where TTargetClass : class
        {
            if (string.IsNullOrWhiteSpace(payload))
                throw new ArgumentNullException(nameof(payload));

            var jsonCompatiblePayload = TransformPayloadToJsonCompatible(payload);
            return TransformJsonCompatiblePayloadToTargetClass<TTargetClass>(jsonCompatiblePayload);
        }

        private static TTargetClass? TransformJsonCompatiblePayloadToTargetClass<TTargetClass>(string jsonCompatiblePayload) where TTargetClass : class =>
            JsonConvert.DeserializeObject<TTargetClass>(jsonCompatiblePayload);

        private static string TransformPayloadToJsonCompatible(string payload)
        {
            var dynamicYaml = DefaultDeserializer.Deserialize<dynamic>(payload);
            return JsonSerialization.Serialize(dynamicYaml);
        }
    }
}