using System;
using System.Collections;
using Common.AssetsSystem;
using Common.UIService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class StartGameState : IGameState
{
    private Logger _logger;
    private UIService _uiService;
    private readonly IAssetProvider _assetProvider;
    private IObjectResolver _container;
    private IAssetUnloader _assetUnloader;
    
    public StartGameState(Logger logger, UIService uiService, IAssetProvider assetProvider, 
        IObjectResolver container, IAssetUnloader assetUnloader)
    {
        _logger = logger;
        _uiService = uiService;
        _assetProvider = assetProvider;
        _container = container;
        _assetUnloader = assetUnloader;
    }

    public async UniTask Enter(StatePayload payload)
    {
        var transition = await _uiService.ShowUIPanelWithComponent<StateTransitionWindowView>("StateTransitionWindow");
        transition.Fade(1500);
        
        var panel = await _assetProvider.GetAssetAsync<GameObject>("GameState");
        var prefab = _container.Instantiate(panel);
        
        _assetUnloader.AddResource(panel);
        _assetUnloader.AttachInstance(prefab);
    }

    public void Update()
    {
       
    }

    public async UniTask Exit()
    {
        _assetUnloader.Dispose();
        _uiService.HideUIPanel();
    }
}