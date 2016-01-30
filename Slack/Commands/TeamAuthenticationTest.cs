﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Analytics.Pipelines.InsertRenderings;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Shell.Framework.Commands;
using Slack.Contracts;
using Slack.Models;
using Slack.Services;

namespace Slack.Commands
{
    public class TeamAuthenticationTest : Command
    {
        #region Fields

        private readonly ISlackMessage _message;
        private readonly ISlackService _service;

        #endregion

        #region Constructors

        public TeamAuthenticationTest()
        {
            _message = new SlackMessage();
            _service = new SlackService();
        }

        public TeamAuthenticationTest(ISlackService service, ISlackMessage message)
        {
            _message = message;
            _service = service;
        }

        #endregion

        public override void Execute(CommandContext context)
        {
            var item = context.Items[0];
            _message.Text = Settings.GetSetting("Slack.TestMessageText", "Hi there from Sitecore!");
            _message.Token = item[TeamContext.TokenFieldId];
            _message.Username = item[TeamContext.UsernameFieldId];
            _message.Channel = Settings.GetSetting("Slack.TestMessageChannel", "general");
            _service.PublishMessage(_message);
        }

        public override CommandState QueryState(CommandContext context)
        {
            var item = context.Items[0];

            return item.TemplateName != TeamContext.TemplateNameStatic ? CommandState.Hidden : base.QueryState(context);
        }
    }
}