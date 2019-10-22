//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using BotWithFlow.Models;
//using Microsoft.Bot.Builder;
//using Microsoft.Bot.Builder.Dialogs;
//using Microsoft.Bot.Schema;

//namespace BotWithFlow.Dialogs
//{
//    public class AddTeam : ComponentDialog
//    {
//        private IStatePropertyAccessor<UserData> _userDataAccessor;

//        public AddTeam(UserState userState) : base(nameof(AddGroupDialog))
//        {
//            _userDataAccessor = userState.CreateProperty<UserData>(nameof(UserData));

//            AddDialog(new TextPrompt(nameof(TextPrompt)));
//            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new List<WaterfallStep>
//            {
//                AskGroupStepAsync,
//                AskGroupNamestepAsync
//            }));

//            InitialDialogId = nameof(WaterfallDialog);
//        }

//        private async Task<DialogTurnResult> AskGroupStepAsync(WaterfallStepContext stepContext,
//            CancellationToken cancellationToken)
//        {
//            var promptMessage = MessageFactory.Text("¿A que grupo va a añadir el equipo?",
//                "¿A que grupo va a añadir el equipo?", InputHints.ExpectingInput);

//            //TODO GET EQUIPOS PICKER SUGGESTED ACTIONS


//            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions {Prompt = promptMessage},
//                cancellationToken);
//        }

//        private async Task<DialogTurnResult> AskGroupNamestepAsync(WaterfallStepContext stepContext,
//            CancellationToken cancellationToken)
//        {
//            var groupId = (string) stepContext.Result;

//            var addTeamDetails = new AddTeamDetails();

//            //TODO CREAR GRUPO LLAMAR A API

//            var promptMessage = MessageFactory.Text("Vale el grupo ha sido creado", InputHints.ExpectingInput);
//            await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions {Prompt = promptMessage},
//                cancellationToken);

//            return await stepContext.EndDialogAsync(true,
//                cancellationToken);
//        }
//    }
//}