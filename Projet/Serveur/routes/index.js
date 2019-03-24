var express = require('express');
var router = express.Router();
var r = require('rethinkdb');

/* GET home page. */
router.get('/', function (req, res, next) {
    r.table("data").filter(
        {"id": "a3fd0daa-a764-464c-a09d-5d7e6f80609e"}  //50054ddb-7c30-4dfd-8429-6275ccb4d1e2
    ).run(app._rdbConn, function (err, result) {
        if (err) throw err;
        result.toArray(function (err, val) {
            if (err) return next(err);
            res.json(val);
        });
    });
});


module.exports = router;
