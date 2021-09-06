var PROTO_PATH = 'D:\\Projects\\chatmop\\Shared\\senkovlad.chat.shared\\chatroom.proto';

var grpc = require('@grpc/grpc-js');
var protoLoader = require('@grpc/proto-loader');
var packageDefinition = protoLoader.loadSync(
  PROTO_PATH,
  {
    keepCase: true,
    longs: String,
    enums: String,
    defaults: true,
    oneofs: true
  });
var charmop_proto = grpc.loadPackageDefinition(packageDefinition).chatroom;

function main() {
  var target = 'localhost:5000';
  var client = new charmop_proto.ChatRoom(target,
    grpc.credentials.createInsecure());

  client.sendMessage({messageText : "JS JS JS JS JS"}, function(err, response){
      console.log('Chat: ', response.message);
  });
}

main();
