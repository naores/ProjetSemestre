var createError = require('http-errors');
var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');

var indexRouter = require('./routes/index');
var usersRouter = require('./routes/users');

app = express();

// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'pug');

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.use('/', indexRouter);
app.use('/users', usersRouter);

// catch 404 and forward to error handler
app.use(function(req, res, next) {
  next(createError(404));
});

// error handler
app.use(function(err, req, res, next) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  // render the error page
  res.status(err.status || 500);
  res.render('error');
});
app = express();

// #### USED FOR CONNEXION MANAGEMENT###
const session = require('express-session');

var sessionMiddleware = session({
  secret: "XXX"
});

app.use(sessionMiddleware);
// ##################

// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'pug');

app.use(express.json());
app.use(express.urlencoded({extended: false}));
app.use(express.static(path.join(__dirname, 'public')));

//DATABASE CONNECTION
const async = require('async');
const r = require('rethinkdb');
const config = require(__dirname + '/config.js');

app.use('/', indexRouter);
const http = require('http').Server(app);


/**
 * Start a connection server listening
 * @param connection
 */
function startExpress(connection) {
  app._rdbConn = connection;
  http.listen(8888);
  console.log('Listening on port ' + config.express.port);
}


/**
 * Waterfall functions that handle database creating and index databse creation
 */
async.waterfall([
  /**
   * Connect the server to the database
   * @param callback
   */
  function connect(callback) {
    r.connect(config.rethinkdb, callback);
  }], function (err, connection) {
  if (err) {
    console.error(err);
    process.exit(1);
    return;
  }
  startExpress(connection);
});

module.exports = app;
