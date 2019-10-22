using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;

namespace BotWithFlow.Bots
{
    public class TeamsBot<T> : ActivityHandler
        where T : Dialog
    {
        private readonly BotState _conversationState;
        private readonly Dialog _dialog;
        private readonly BotState _userState;

        public TeamsBot(ConversationState conversationState, UserState userState, T dialog)
        {
            _dialog = dialog;
            _conversationState = conversationState;
            _userState = userState;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occured during the turn.
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {
            await _dialog.RunAsync(turnContext, _conversationState.CreateProperty<DialogState>(nameof(DialogState)),
                cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded,
            ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;
            if (activity.ChannelId == Channels.Webchat || activity.ChannelId == Channels.Directline)
                return;
            foreach (var member in membersAdded)
                // Greet anyone that was not the target (recipient) of this message.
                // To learn more about Adaptive Cards, see https://aka.ms/msbot-adaptivecards for more details.
                if (member.Id != turnContext.Activity.Recipient.Id)
                    await turnContext.SendActivityAsync($"Hola! - {member.Name}. empecemos a trabajar juntos.",
                        cancellationToken: cancellationToken);
        }
    }
}