﻿using NUnit.Framework;
using Pulse.Core.Components;
using Pulse.Core.Models;

namespace Pulse.Core.Tests
{
    [TestFixture]
    [Category("EventManagerCore")]
    public class EventManagerTests
    {
        private EventManager _manager;

        [SetUp]
        public void Init()
        {
            _manager = new EventManager();
        }

        [Test]
        public void IsQueueInstantiatedEmpty()
        {
            Assert.AreEqual(0, _manager.QueueSize());
        }

        [Test]
        public void IsDelegatesEmptyOnStart()
        {
            Assert.IsTrue(_manager.HasListeners());
        }

        public void OnCall(GameEvent evt)
        {
        }

        [Test]
        public void DoesAddListenerAttachListener()
        {
            _manager.AddListener<GameEvent>(OnCall);
            Assert.IsTrue(_manager.HasListener<GameEvent>(OnCall));
        }

        [Test]
        public void DoesAddListenerAddToDelegates()
        {
            _manager.AddListener<GameEvent>(OnCall);
            Assert.IsFalse(_manager.HasListeners());
        }

        [Test]
        public void DoesAddListenerOnceAttachListener()
        {
            _manager.AddListenerOnce<GameEvent>(OnCall);
            Assert.IsTrue(_manager.HasListener<GameEvent>(OnCall));
        }

        [Test]
        public void DoessAddListenerOnceAddToDelegates()
        {
            _manager.AddListenerOnce<GameEvent>(OnCall);
            Assert.IsFalse(_manager.HasListeners());
        }

        [Test]
        public void DoesRemoveListenerRemoveDelegate()
        {
            _manager.AddListener<GameEvent>(OnCall);
            _manager.RemoveListener<GameEvent>(OnCall);
            Assert.IsFalse(_manager.HasListener<GameEvent>(OnCall));
        }

        [Test]
        public void DoesRemoveListenerClearDelegates()
        {
            _manager.AddListener<GameEvent>(OnCall);
            _manager.RemoveListener<GameEvent>(OnCall);
            Assert.IsTrue(_manager.HasListeners());
        }

        [Test]
        public void DoesRemoveListenerOnOnceRemoveDelegate()
        {
            _manager.AddListenerOnce<GameEvent>(OnCall);
            _manager.RemoveListener<GameEvent>(OnCall);
            Assert.IsFalse(_manager.HasListener<GameEvent>(OnCall));
        }

        [Test]
        public void DoesRemoveListenerOnOnceClearDelegates()
        {
            _manager.AddListenerOnce<GameEvent>(OnCall);
            _manager.RemoveListener<GameEvent>(OnCall);
            Assert.IsTrue(_manager.HasListeners());
        }

        [Test]
        public void DoesRemoveAllClearListeners()
        {
            _manager.AddListener<GameEvent>(OnCall);
            _manager.RemoveListeners();
            Assert.IsFalse(_manager.HasListener<GameEvent>(OnCall));
        }

        [Test]
        public void DoesRemoveAllClearDelegates()
        {
            _manager.AddListener<GameEvent>(OnCall);
            _manager.RemoveListeners();
            Assert.IsTrue(_manager.HasListeners());
        }

        [Test]
        public void DoesRemoveClearAllEventsQueued()
        {
            _manager.QueueEvent(new GameEvent());
            _manager.RemoveListeners();
            Assert.IsTrue(_manager.IsQueueEmpty());
        }

        [Test]
        public void DoesQueueEventAddToQueue()
        {
            _manager.AddListener<GameEvent>(OnCall);
            _manager.QueueEvent(new GameEvent());
            Assert.AreEqual(1, _manager.QueueSize());
        }

        [Test]
        public void DoesQueueEventTriggerEventOnUpdate()
        {
            _manager.QueueEvent(new GameEvent());
            _manager.ProcessEvents(1);
        }

        [Test]
        public void DoesAddListenerOnceGetRemovedAfterOneEvent()
        {
            //Arrange
            _manager.AddListenerOnce<GameEvent>(OnCall);
            _manager.QueueEvent(new GameEvent());

            //Act
            _manager.ProcessEvents(1);

            //Assert
            Assert.AreEqual(0, _manager.QueueSize());
        }

        [Test]
        public void DoesAddListenerOnceWhenYouReAddTheSameListener()
        {
            //Arrange
            _manager.AddListenerOnce<GameEvent>(OnCall);
            _manager.QueueEvent(new GameEvent());

            //Act
            _manager.ProcessEvents(1);
            _manager.AddListenerOnce<GameEvent>(OnCall);

            //Assert
            Assert.IsTrue(_manager.HasListener<GameEvent>(OnCall));
        }

        [Test]
        public void DoesAListenOnceReaddedToSoonCauseNoListeners()
        {
            //Arrange
            _manager.AddListenerOnce<GameEvent>(OnCall);
            _manager.QueueEvent(new GameEvent());

            //Act
            _manager.AddListenerOnce<GameEvent>(OnCall);
            _manager.ProcessEvents(1);

            //Assert
            Assert.IsFalse(_manager.HasListener<GameEvent>(OnCall));
        }
    }
}