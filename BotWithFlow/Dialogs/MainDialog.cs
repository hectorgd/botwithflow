// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BotWithFlow.Luis;
using BotWithFlow.Models;
using Flow.Client.API.Models;
using Luis;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace BotWithFlow.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly TeamsActionRecognizer _teamsActionRecognizer;
        private IStatePropertyAccessor<UserData> _userDataAccessor;

        public MainDialog(TeamsActionRecognizer teamsActionRecognizer,
            UserState userState, AddGroupDialog addGroupDialog) : base(nameof(MainDialog))

        {
            _teamsActionRecognizer = teamsActionRecognizer;

            _userDataAccessor = userState.CreateProperty<UserData>(nameof(UserData));

            AddDialog(addGroupDialog);

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog),
                new List<WaterfallStep>
                {
                    StartAsync
                }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> StartAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            stepContext.Context.Activity.Text = stepContext.Context.Activity.Text.Replace("<at>Flowy</at>", "");
            // Call LUIS and gather intent (Note the TurnContext has the response to the prompt.)
            var luisResult =
                await _teamsActionRecognizer.RecognizeAsync<LuisWithFlow>(stepContext.Context, cancellationToken);
            switch (luisResult.TopIntent().intent)
            {
                case LuisWithFlow.Intent.AddGroup:

                    // Initialize CreateGroupParams with any entities we may have found in the response.
                    var groupParams = new CreateGroupParams();

                    //Add as owner the person who is making the request
                    groupParams.teamOwnersoDatabind = string.Format("https://graph.microsoft.com/v1.0/users/{0}",
                        stepContext.Context.Activity.From.AadObjectId);

                    //Check if groupname comes from LUIS result
                    if (luisResult.TryGetGroupName(out var groupName)) groupParams.teamDisplayName = groupName;

                    // Run the AddGroupDialog giving it whatever details we have from the LUIS call, it will fill out the remainder.
                    return await stepContext.BeginDialogAsync(nameof(AddGroupDialog), groupParams,
                        cancellationToken);
                case LuisWithFlow.Intent.Greetings:
                    var greetings = MessageFactory.Text($"Hola, ¿Qué tal {stepContext.Context.Activity.From.Name}?");
                    await stepContext.Context.SendActivityAsync(greetings, cancellationToken);
                    return await stepContext.NextAsync(null, cancellationToken);
                case LuisWithFlow.Intent.GreetingsPerson:
                    if (luisResult.TryGetPersonName(out var personName))
                    {
                        var greetingsPerson =
                            MessageFactory.Text(
                                $"Hola {personName}. {stepContext.Context.Activity.From.Name} quiere que te salude ¯\\_(ツ)_/¯");
                        await stepContext.Context.SendActivityAsync(greetingsPerson, cancellationToken);
                    }

                    return await stepContext.NextAsync(null, cancellationToken);
                default:
                    return await stepContext.NextAsync(null, cancellationToken);
            }
        }
    }
}