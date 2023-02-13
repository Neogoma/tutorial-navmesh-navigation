# tutorial-navmesh-navigation

This repository covers the use of Unity3d Navmesh Components together with the stardust SDK for visual positioning in a tutorial where we use a guide bot to display some information about a desired target location.
Enabling developers to anchor immersive experiences in space, as well as allowing for other to view the experience.

Link to video tutorial: https://www.youtube.com/watch?v=_7BB3cneXXw&ab_channel=Neogoma%28%23StardustSDK%29

Setup:
- You need to use your API key, get it from your Stardust settings dashboard: https://stardust.neogoma.com/login
- A glb/point cloud for reference.
- Create a prefab variant of the navMeshPrefab.
- Set up your NavMesh transform to the appropriate location and size.
- Add a target to the Navigation Robot prefab, as well as a hint to display when target is reached.
- Add your map UUID to the nav mesh prefab variant.
- Add your prefab variant to the MainNavPrefab asset.
- Clear any residual objects from the hierarchy.

Note: Clone the repo, downloading the zip file might result in missing files. 


Documentation for the Stardust SDK here: https://doc.neogoma.com/#/
