using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChessTimer
{
    public class Function
    {
        GameResource EnGameResource { get; set; }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();

            if (EnGameResource == null)
            {
                var allResources = GetResources();
                EnGameResource = allResources.Where(x => x.Language == "en-US").FirstOrDefault();
            }

            response.Response = new ResponseBody
            {
                ShouldEndSession = false
            };
            IOutputSpeech innerResponse = null;
            var log = context.Logger;

            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                log.LogLine($"Default LaunchRequest made: 'Alexa, open Chess Timer");
                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = EnGameResource.OpenMessage;
            }
            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                IntentRequest intentRequest = (IntentRequest)input.Request;
                switch (intentRequest.Intent.Name)
                {
                    case "AMAZON.CancelIntent":
                        log.LogLine($"AMAZON.CancelIntent: send StopMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = EnGameResource.StopMessage;
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.StopIntent":
                        log.LogLine($"AMAZON.StopIntent: send StopMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = EnGameResource.StopMessage;
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.HelpIntent":
                        log.LogLine($"AMAZON.HelpIntent: send HelpMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = EnGameResource.HelpMessage;
                        break;
                    case "GetPlayerAmountIntent":
                        log.LogLine($"AddPlayerIntent sent: add new player");
                        int amount = Int32.Parse(intentRequest.Intent.Slots["PlayerAmount"].Value);
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = EnGameResource.GetPlayesInfo(amount);
                        break;
                    case "StartNewGameIntent":
                        log.LogLine($"StartNewGameIntent sent: start new game");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = EnGameResource.StartGames();
                        break;
                    case "NextTurnIntent":
                        log.LogLine($"NextTurnIntent sent: next turn");
                        string playerNext = intentRequest.Intent.Slots["PlayerName"].Value;
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = EnGameResource.StartNextPlayerTurn(playerNext);
                        break;
                    case "PauseTurnIntent":
                        log.LogLine($"NextTurnIntent sent: next turn");
                        string playerPause = intentRequest.Intent.Slots["PlayerName"].Value;
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = EnGameResource.PauseGame(playerPause);
                        break;
                    case "ContinueTurnIntent":
                        log.LogLine($"NextTurnIntent sent: next turn");
                        string playerContinue = intentRequest.Intent.Slots["PlayerName"].Value;
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = EnGameResource.ContinueGame(playerContinue);
                        break;
                    case "GetInfoIntent":
                        log.LogLine($"GetInfoIntent sent: get information");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = EnGameResource.GetGameInfo();
                        break;
                    default:
                        log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = EnGameResource.HelpReprompt;
                        break;
                }                
            }
            response.Response.OutputSpeech = innerResponse;
            response.Version = "1.0";
            return response;
        }

        /// <summary>
        /// Получение списка ресурсов.
        /// </summary>
        /// <returns></returns>
        public List<GameResource> GetResources()
        {
            List<GameResource> resources = new List<GameResource>();
            GameResource enUSResource = new GameResource("en-US")
            {
                SkillName = "Chess timer",
            };

            resources.Add(enUSResource);
            return resources;
        }
    }   
}
