using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BotWithFlow.Models;
using Flow.Client.API.Interfaces;
using Flow.Client.API.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace BotWithFlow.Dialogs
{
    public class AddGroupDialog : ComponentDialog
    {
        private readonly IFlowClient _flowClient;
        private IStatePropertyAccessor<UserData> _userDataAccessor;

        public AddGroupDialog(UserState userState, IFlowClient flowClient) : base(nameof(AddGroupDialog))
        {
            _flowClient = flowClient;
            _userDataAccessor = userState.CreateProperty<UserData>(nameof(UserData));

            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new List<WaterfallStep>
            {
                AskNameStepAsync,
                AskDescriptionStepAsync,
                AskMailNicknameStepAsync,
                AskChannelName,
                AskChannelDescription,
                AskTabName,
                AskTabUrl,
                AskUsers,
                EndDialog
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> AskNameStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            var groupDetails = (CreateGroupParams)stepContext.Options;
            if (string.IsNullOrEmpty(groupDetails.teamDisplayName))
            {
                var promptMessage = MessageFactory.Text("¿Cómo quieres llamar al grupo?",
                    "¿Cómo quieres llamar al grupo?", InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage },
                    cancellationToken);
            }

            return await stepContext.NextAsync(groupDetails.teamDisplayName, cancellationToken);
        }

        private async Task<DialogTurnResult> AskDescriptionStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            var groupDetails = (CreateGroupParams)stepContext.Options;
            groupDetails.teamDisplayName = (string)stepContext.Result;

            var promptMessage = MessageFactory.Text("¿Alguna descripción chula?", "¿Alguna descripción chula?",
                InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage },
                cancellationToken);
        }

        private async Task<DialogTurnResult> AskMailNicknameStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            var groupDetails = (CreateGroupParams)stepContext.Options;
            groupDetails.teamDescription = (string)stepContext.Result;

            var promptMessage = MessageFactory.Text("Bien , ahora necesito el mailNickname del grupo, ¿Cual sería?",
                "Bien, ahora necesito el mailNickname del grupo, ¿Cual sería?", InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage },
                cancellationToken);
        }

        private async Task<DialogTurnResult> AskChannelName(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            var groupDetails = (CreateGroupParams)stepContext.Options;
            groupDetails.teamMailNickname = (string)stepContext.Result;

            var promptMessage = MessageFactory.Text("Ahora pasamos a los canalales, ¿Qué nombre le vamos a dar?",
                "Ahora pasamos a los canalales, ¿Qué nombre le vamos a dar?", InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage },
                cancellationToken);
        }

        private async Task<DialogTurnResult> AskChannelDescription(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            var groupDetails = (CreateGroupParams)stepContext.Options;
            groupDetails.channelsDisplayName = (string)stepContext.Result;

            var promptMessage = MessageFactory.Text("¿Y una breve descripción para el canal cual sería?",
                "¿Y una breve descripción para el canal cual sería?", InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage },
                cancellationToken);
        }

        private async Task<DialogTurnResult> AskTabName(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            var groupDetails = (CreateGroupParams)stepContext.Options;
            groupDetails.channelsDescription = (string)stepContext.Result;

            var promptMessage = MessageFactory.Text("¿Que nombre le daremos a la pestaña?",
                "¿Que nombre le daremos a la pestaña?", InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage },
                cancellationToken);
        }

        private async Task<DialogTurnResult> AskTabUrl(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            var groupDetails = (CreateGroupParams)stepContext.Options;
            groupDetails.tabsDisplayName = (string)stepContext.Result;

            var promptMessage = MessageFactory.Text("¿Que Url mostrara la pestaña?",
                "¿Que Url mostrara la pestaña?", InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage },
                cancellationToken);
        }

        private async Task<DialogTurnResult> AskUsers(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var groupDetails = (CreateGroupParams)stepContext.Options;
            groupDetails.tabsWebsiteUrl = (string)stepContext.Result;

            List<Attachment> usersCardAttachments = new List<Attachment>();

            var users = _flowClient.GetUsers();
            foreach (var userValue in users.value)
            {
                var userCard = new ThumbnailCard
                {
                    Title = userValue.displayName,
                    Subtitle = userValue.id,
                    Images = new List<CardImage> { new CardImage { Url = "https://botdevhecdiag.blob.core.windows.net/public/id-card.png" } }
                };
                usersCardAttachments.Add(userCard.ToAttachment());
            }

            Activity message = MessageFactory.Carousel(usersCardAttachments,
                "Por último, ¿Que usuarios perteneceran a este grupo?, Pegame las ids separadas por ;") as Activity;

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = message }, cancellationToken);
        }


        private Task<DialogTurnResult> EndDialog(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            var userResult = stepContext.Result as string;
            var userList = userResult.Split(';');

            var groupDetails = (CreateGroupParams)stepContext.Options;
            groupDetails.membersodatabind = new List<string>();

            foreach (var userId in userList)
            {
                var replacedId = Regex.Replace(userId, @"\t|\n|\r", "");
                groupDetails.membersodatabind.Add(string.Format("https://graph.microsoft.com/v1.0/users/{0}", replacedId));
            }

            try
            {
                var response = _flowClient.CreateGroup(groupDetails);

                var message = MessageFactory.Text("El grupo ha sido creado con exito");
                stepContext.Context.SendActivityAsync(message, cancellationtoken);
                return stepContext.EndDialogAsync(true, cancellationtoken);
            }
            catch (Exception e)
            {
                var message = MessageFactory.Text("Ups algo no ha ido bien , el grupo no se ha creado");
                stepContext.Context.SendActivityAsync(message, cancellationtoken);
                return stepContext.EndDialogAsync(false, cancellationtoken);
            }
        }
    }
}