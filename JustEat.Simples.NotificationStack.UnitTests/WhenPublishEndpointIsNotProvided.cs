﻿using JustSaying.AwsTools;
using JustSaying.AwsTools.QueueCreation;
using JustEat.Testing;
using NUnit.Framework;

namespace Stack.UnitTests
{
    class WhenPublishEndpointIsNotProvided : BehaviourTest<SqsConfiguration>
    {
        protected override void Given()
        {
            RecordAnyExceptionsThrown();
        }

        protected override void When()
        {
            SystemUnderTest.Validate();
        }

        [Then]
        public void ThrowsException()
        {
            Assert.IsNotNull(ThrownException);
        }

        protected override SqsConfiguration CreateSystemUnderTest()
        {
            return new SqsConfiguration() { MessageRetentionSeconds = JustSayingConstants.MINIMUM_RETENTION_PERIOD +1, Topic = "ATopic", PublishEndpoint = null };
        }
    }
}
