/**
 * Config for the database connection and server listening port
 * @type {{rethinkdb: {host: string, port: number, authKey: string, db: string}, express: {port: number}}}
 */
module.exports = {
    rethinkdb: {
        host: "localhost",
        port: 28015,
        authKey: "",
        db: "ProjetSem"
    },
    express: {
        port : 8888
    }

}