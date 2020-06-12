## FireFighter Game for Game Engine I

### User Story 1
- [x] If the number of fires is equal to 0, we need to trigger a victory end screen to display results. If we hit any key, we load the next level (or the same level again if we only have one).
- [x] We define how many fires are lit per level (1 to 6)

### User Story 2
- [ ] The game starts with a specific number of fire (1 to 6), after a delay (20 seconds or less) a new fire will start at a random window (that did not have a fire before.)  
- [ ] After we end the level, we give points (between 50 to 150 pts) based on the windows that did not caught on fire. ((14 – totalFires) * preventionScore) 
- [ ] We need to display the number of active fires in the UI.
### User Story 3
- [ ] We need to measure the time from the beginning of the level and stop the clock when we finish.
- [ ] We need to define an average time for each level (manually set, based on previous attempt). 
- [ ] We need to define a base score to grade the player (between 150 to 500 pts). 
- [ ] When we finish a level, we display a summary and a score based on how long it took to complete the level. ((avgTime / timeElapsed) * timeScore);
- [ ] We need to display the results with details in the victory end screen. 
- [ ] We need to display the elapse time in the UI. 
### User Story 4
- [ ] We need to measure the damage amount based on how many fires are lit, and for how long (between 0.5 to 5% / second).  damage += (damageSpeed * activeFire) * Time.deltaTime;
- [ ] When we complete a level, we need to give a score penalty (-10 pts/ 1%). **_Final score cannot be less than 0!_** (damage * penaltyScore); 
- [ ] If we reach 100% damage, the game ends with a Game Over screen. 
- [ ] When Game Over, we can restart the game from level 1 or exit game. 
- [ ] We need to display the damage in the UI. 
### User Story 5
- [ ] We need to specify an amount of water consumed per level (between 0.05 and 1%/second active). **_The level only goes down when we use it._**
- [ ] If we ran out of water, the game ends with Game Over. 
- [ ] We need to specify different amount per level, to make the game more intense. 
- [ ] Every level must be beatable! – Test your late levels to make sure it is playable. 
- [ ] When we complete a level, we need to display a score based on the amount of water remaining (1000 pts/1%). (waterLeft * scoreWater); 
- [ ] We need to display the amount of water remaining in the UI. 
### User Story 6
- [ ] Slowly increase the intensity back when the value is below the original intensity (3) if we are not trying to extinguish it. 
- [ ] Invoke the increase only after a delay (1 second) when we previously tried to extinguish the fire.
### User Story 7
- [ ] Add a high score in PlayerPrefs for each level. 
- [ ] Display the best score in the victory screen.
### User Story 8
- [ ] Add a main menu with 2 buttons: Start Game, Exit game.
- [ ] When we’re Game Over, we need to replace the button Exit game by Go to Main menu. 
- [ ] Add loading screen between scenes in the Build Settings. 
- [ ] The UI must always look good on different resolutions screen and aspect ratio. 
### User Story 9
- [ ] Create between 5 to 10 levels with increasingly more difficulty in the game manager. 
- [ ] Create an ending screen to say congratulations to winning player and then add a button to return to the main menu (or quit the game if you don’t have any.)
### User Story 10
- [ ] Add an icon for the build.
- [ ] Add a Config dialog image.
- [ ] Remove unused inputs in the input manager. 

# Credits
**All assets provided by Marc-André Larouche**