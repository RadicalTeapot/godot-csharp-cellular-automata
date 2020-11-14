# typescript-experiments

Collections of single html page experiments using typescript and [expressJS](http://expressjs.com/) server.

## Notes

### Server routing

Since transpiled typescript code imports javascript modules using paths without a file extension, the server routing has to autofill missing extensions when serving files from the scripts folder with `.js`.

## Pages

### Game

Pages with game related content.

#### simple_platformer

Bare-bones platformer implementation based on [this implementation](https://medium.com/@btco_code/writing-a-platformer-for-the-tic-80-virtual-console-6fa737abe476).

### Other

Pages with content which doesn't fit in other categories.

#### hello_typescript

Hello world using typescript.
