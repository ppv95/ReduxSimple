﻿using Shouldly;
using System;
using Xunit;
using static ReduxSimple.Tests.Setup.TodoListStore.Functions;
using TodoListStore = ReduxSimple.ReduxStore<ReduxSimple.Tests.Setup.TodoListStore.TodoListState>;

namespace ReduxSimple.Tests
{
    public class RedoTest
    {
        [Fact]
        public void CanRedo()
        {
            // Arrange
            var initialState = CreateInitialTodoListState();
            var store = new TodoListStore(
                Setup.TodoListStore.Reducers.CreateReducers(),
                initialState,
                true
            );

            // Act
            DispatchAllActions(store);

            store.CanUndo.ShouldBeTrue();

            store.Undo();

            store.CanUndo.ShouldBeTrue();

            store.Undo();

            store.CanUndo.ShouldBeTrue();

            store.Undo();

            store.CanRedo.ShouldBeTrue();

            store.Redo();

            store.CanRedo.ShouldBeTrue();

            store.Redo();

            // Assert
            store.State.TodoList?.Count.ShouldBe(2);
            store.State.CurrentUser.ShouldBe("Emily");
        }

        [Fact]
        public void CannotRedo()
        {
            // Arrange
            var initialState = CreateInitialTodoListState();
            var store = new TodoListStore(
                Setup.TodoListStore.Reducers.CreateReducers(),
                initialState,
                true
            );

            // Act
            DispatchAllActions(store);

            // Assert
            store.CanRedo.ShouldBeFalse();
        }
        
        [Fact]
        public void RedoAndObserveNormalTimelineActionsOnly()
        {
            // Arrange
            var initialState = CreateInitialTodoListState();
            var store = new TodoListStore(
                Setup.TodoListStore.Reducers.CreateReducers(),
                initialState,
                true
            );

            // Act
            int observeCount = 0;

            store.ObserveAction()
                .Subscribe(_ =>
                {
                    observeCount++;
                });

            DispatchAllActions(store);

            store.Undo();
            store.Undo();
            store.Undo();
            store.Redo();
            store.Redo();

            // Assert
            observeCount.ShouldBe(4);
        }

        [Fact]
        public void RedoAndObserveRedoneActionsOnly()
        {
            // Arrange
            var initialState = CreateInitialTodoListState();
            var store = new TodoListStore(
                Setup.TodoListStore.Reducers.CreateReducers(),
                initialState,
                true
            );

            // Act
            int observeCount = 0;

            store.ObserveAction(ActionOriginFilter.Redone)
                .Subscribe(_ =>
                {
                    observeCount++;
                });

            DispatchAllActions(store);

            store.Undo();
            store.Undo();
            store.Undo();
            store.Redo();
            store.Redo();

            // Assert
            observeCount.ShouldBe(2);
        }
    }
}
