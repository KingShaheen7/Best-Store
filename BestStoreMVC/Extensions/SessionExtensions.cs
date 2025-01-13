using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BestStoreMVC.Extensions // Replace this with your project namespace
{
	public static class SessionExtensions
	{
		// Store an object in session after serializing it to JSON
		public static void Set<T>(this ISession session, string key, T value)
		{
			session.SetString(key, JsonSerializer.Serialize(value));
		}

		// Retrieve an object from session and deserialize it from JSON
		public static T Get<T>(this ISession session, string key)
		{
			var value = session.GetString(key);
			return value == null ? default : JsonSerializer.Deserialize<T>(value);
		}
	}
}
