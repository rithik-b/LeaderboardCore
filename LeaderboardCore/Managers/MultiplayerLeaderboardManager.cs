using System;
using HMUI;
using IPA.Utilities;
using Zenject;

namespace LeaderboardCore.Managers
{
	internal class MultiplayerLeaderboardManager : IInitializable, IDisposable
	{
		private readonly MainFlowCoordinator _mainFlowCoordinator;
		private readonly MultiplayerLobbyController _multiplayerLobbyController;
		private readonly ServerPlayerListViewController _serverPlayerListViewController;
		private readonly PlatformLeaderboardViewController _platformLeaderboardViewController;
		private readonly LevelSelectionNavigationController _levelSelectionNavigationController;

		public MultiplayerLeaderboardManager(MainFlowCoordinator mainFlowCoordinator, MultiplayerLobbyController multiplayerLobbyController, ServerPlayerListViewController serverPlayerListViewController, PlatformLeaderboardViewController platformLeaderboardViewController, LevelSelectionNavigationController levelSelectionNavigationController)
		{
			_mainFlowCoordinator = mainFlowCoordinator;
			_multiplayerLobbyController = multiplayerLobbyController;
			_serverPlayerListViewController = serverPlayerListViewController;
			_platformLeaderboardViewController = platformLeaderboardViewController;
			_levelSelectionNavigationController = levelSelectionNavigationController;
		}

		private void ShowLeaderboard(IDifficultyBeatmap difficultyBeatmap)
		{
			if (!_multiplayerLobbyController.lobbyActivated)
				return;

			if (difficultyBeatmap is null)
			{
				HideLeaderboard();
				return;
			}

			_platformLeaderboardViewController.SetData(_levelSelectionNavigationController.selectedDifficultyBeatmap);
			
			_mainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf().InvokeMethod<object, FlowCoordinator>("SetRightScreenViewController", _platformLeaderboardViewController, ViewController.AnimationType.In);
			_serverPlayerListViewController.gameObject.SetActive(false);
		}

		private void HideLeaderboard()
		{
			_mainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf().InvokeMethod<object, FlowCoordinator>("SetRightScreenViewController", null, ViewController.AnimationType.Out);
		}
		
		private void LevelSelectionNavigationControllerOndidChangeLevelDetailContentEvent(LevelSelectionNavigationController levelSelectionNavigationController, StandardLevelDetailViewController.ContentType contentType)
		{
			if (contentType == StandardLevelDetailViewController.ContentType.OwnedAndReady)
			{
				ShowLeaderboard(levelSelectionNavigationController.selectedDifficultyBeatmap);
				return;
			}
			
			ShowLeaderboard(null);
		}

		private void LevelSelectionNavigationControllerOndidChangeDifficultyBeatmapEvent(LevelSelectionNavigationController levelSelectionNavigationController, IDifficultyBeatmap difficultyBeatmap)
		{
			ShowLeaderboard(difficultyBeatmap);
		}

		public void Initialize()
		{
			_levelSelectionNavigationController.didChangeDifficultyBeatmapEvent += LevelSelectionNavigationControllerOndidChangeDifficultyBeatmapEvent;
			_levelSelectionNavigationController.didChangeLevelDetailContentEvent += LevelSelectionNavigationControllerOndidChangeLevelDetailContentEvent;
		}
		
		public void Dispose()
		{
			_levelSelectionNavigationController.didChangeDifficultyBeatmapEvent -= LevelSelectionNavigationControllerOndidChangeDifficultyBeatmapEvent;
			_levelSelectionNavigationController.didChangeLevelDetailContentEvent -= LevelSelectionNavigationControllerOndidChangeLevelDetailContentEvent;
		}
	}
}