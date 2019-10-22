// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BotWithFlow.Luis;
using BotWithFlow.Models;
using Flow.Client.API.Models;
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
            // Call LUIS and gather intent (Note the TurnContext has the response to the prompt.)
            var luisResult = await _teamsActionRecognizer.RecognizeAsync(stepContext.Context, cancellationToken);
            switch (luisResult.GetTopScoringIntent().intent)
            {
                case "AddGroup":

                    // Initialize CreateGroupParams with any entities we may have found in the response.
                    var groupParams = new Flow.Client.API.Models.CreateGroupParams();

                    // Run the BookingDialog giving it whatever details we have from the LUIS call, it will fill out the remainder.
                    return await stepContext.BeginDialogAsync(nameof(AddGroupDialog), groupParams,
                        cancellationToken);
                default:
                    return await stepContext.NextAsync(null, cancellationToken);
            }
        }
    }
}