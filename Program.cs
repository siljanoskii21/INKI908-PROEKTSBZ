using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

class TravelDestinationRecommendationSystem
{
    private Dictionary<string, string> userPreferences = new Dictionary<string, string>();
    private Dictionary<string, string> recommendations = new Dictionary<string, string>();

    public void LoadKnowledgeBase(string fileName)
    {
        userPreferences.Clear();
        recommendations.Clear();

        string jsonContent = File.ReadAllText(fileName);
        var knowledgeBase = JsonConvert.DeserializeObject<JObject>(jsonContent);

        var factsArray = knowledgeBase["Facts"].ToObject<JArray>();
        foreach (var fact in factsArray)
        {
            string name = fact["Name"].ToString();
            string question = fact["Question"].ToString();

            // Direct input without validation loop
            Console.Write($"{question} (Yes/No): ");
            string answer = Console.ReadLine()?.Trim() ?? "";

            userPreferences[name] = answer;
        }

        var recommendationsObj = knowledgeBase["Recommendations"].ToObject<JObject>();
        foreach (var recommendation in recommendationsObj.Properties())
        {
            string name = recommendation.Name;
            string message = recommendation.Value.ToString();
            recommendations[name] = message;
        }
    }

    public void MakeRecommendation()
    {
        Console.WriteLine("\n--- RECOMMENDATION ---");
        foreach (var preference in userPreferences)
        {
            string val = preference.Value?.Trim() ?? "";
            
            if ((val.Equals("Yes", StringComparison.OrdinalIgnoreCase) || val.Equals("Y", StringComparison.OrdinalIgnoreCase)) 
                && recommendations.ContainsKey(preference.Key))
            {
                Console.WriteLine(recommendations[preference.Key]);
                return;
            }
        }

        if (recommendations.ContainsKey("Default"))
        {
            Console.WriteLine(recommendations["Default"]);
        }
        else
        {
            Console.WriteLine("We recommend a diverse and exciting travel destination!");
        }
    }
}

class Program
{
    static void Main()
    {
        TravelDestinationRecommendationSystem recommendationSystem = new TravelDestinationRecommendationSystem();
        
        string jsonPath = "knowledgebase.json";
        if (!File.Exists(jsonPath))
        {
            jsonPath = @"C:\Users\KRISTIJAN\Desktop\Project\knowledgebase.json";
        }

        Console.Clear();
        Console.WriteLine("=== Travel Destination Recommendation System ===\n");
        
        recommendationSystem.LoadKnowledgeBase(jsonPath);
        recommendationSystem.MakeRecommendation();

        Console.WriteLine("\nThank you for using the application! Press any key to exit...");
        Console.ReadKey();
    }
}