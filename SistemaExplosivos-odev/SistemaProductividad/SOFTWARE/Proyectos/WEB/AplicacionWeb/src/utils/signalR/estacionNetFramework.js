import { hubConnection } from "signalr-asp-net";

class SignalREstacionNetFramework {
  constructor(ipEstacion, nombreHub, nombreMetodoRecibirDatos){
    this.nombreHub = nombreHub;
    this.ipEstacion = ipEstacion;
    this.nombreMetodoRecibirDatos = nombreMetodoRecibirDatos;
    this.proxyHub = null;
    this.detenerConexion = false;
    this.signalRConexionHub = null;
    this.fMensajeRecibido = () => {};
    this.fEnCambioEstatusConexion = () => {};
    this.fEnConexion = () => {};
  }

  configurarConexionHub(fMensajeRecibido, fEnCambioEstatusConexion, fEnConexion){

    //Genera mediante la clase de hubconnection de .NET CORE un hub
    this.signalRConexionHub = hubConnection(`http://${this.ipEstacion}`);
    this.proxyHub = this.signalRConexionHub.createHubProxy(this.nombreHub);

    //Asigna las funciones que se agregaron como argumentos para poder ejecutarlas desde
    //dentro de la clase
    //Es la función que se debe ejecutar cuando se detecta un cambio en el estatus de la conexión.
    //Por ahora solo indica cuando pasa a estar desconectado y conectado
    this.fEnCambioEstatusConexion = fEnCambioEstatusConexion;

    //Función que se dispara solo cuando se conecta a una estación.
    this.fEnConexion = fEnConexion;

    this.signalRConexionHub.logging = true;

    this.signalRConexionHub.starting(() => {});

    this.signalRConexionHub.connectionSlow(() => {});

    //Aqui se pueden añadir funciones que se quiera que se disparen en caso de un error.
    //Por ahora indica que al existir un error no hay conexión con el hub
    this.signalRConexionHub.error(() => {
      if(this.fEnCambioEstatusConexion) this.fEnCambioEstatusConexion(false);
    });

    //Aqui se pueden añadir funciones que se quiera que se disparen en caso de estar reconectando.
    //Por ahora indica que durante el proceso de reconexión no hay conexión con el hub
    this.signalRConexionHub.reconnecting(() => {
      if(this.fEnCambioEstatusConexion) this.fEnCambioEstatusConexion(false);
    });

    //Aqui se pueden añadir funciones que se quiera que se disparen en caso de se concrete una reconexión.
    //Por ahora indica que al reconectar hay conexión con el hub
    this.signalRConexionHub.reconnected(() => {
      if(this.fEnCambioEstatusConexion) this.fEnCambioEstatusConexion(true);
    });

    this.signalRConexionHub.stateChanged(() => {
      if(this.signalRConexionHub.state == 4){
        if(this.fEnCambioEstatusConexion) this.fEnCambioEstatusConexion(false);
      }
      if(this.signalRConexionHub.state == 1){
        if(this.fEnCambioEstatusConexion) this.fEnCambioEstatusConexion(true);
        if(this.fEnConexion) this.fEnConexion();
      }
    });

    this.signalRConexionHub.disconnected(() => {
      if(!this.detenerConexion){
        setTimeout(() => {
            if(this.signalRConexionHub){
              this.signalRConexionHub.start({transport: ["webSockets", "longPolling"], jsonp: true })
                .done(() => {
                  this.signalRConexionHub.transport.reconnectDelay=10000;
                })
                .fail(() => {
                  console.error("Failed to restart SignalR connection.");
                });
            }
          }, 10000);
      }
    });

    this.proxyHub.on(this.nombreMetodoRecibirDatos, fMensajeRecibido);

    this.signalRConexionHub.start({transport: ["webSockets", "longPolling"], jsonp: true })
      .done(() => {
        this.signalRConexionHub.transport.reconnectDelay=10000;
        if(this.fEnCambioEstatusConexion) this.fEnCambioEstatusConexion(true);
      })
      .fail(() => {
    });
  }

  async iniciarConexionHub(){
    try{
      if(this.signalRConexionHub){
        if(this.signalRConexionHub.state == 4){
          this.signalRConexionHub.start({transport: ["webSockets", "longPolling"], jsonp: true })
            .done(() => {
              this.signalRConexionHub.transport.reconnectDelay=10000;
              if(this.fEnCambioEstatusConexion) this.fEnCambioEstatusConexion(true);
              if(this.fEnConexion) this.fEnConexion();
            })
            .fail(() => {
              if(this.fEnCambioEstatusConexion) this.fEnCambioEstatusConexion(false)
          });
        }
      }
    }catch {
      if(this.fEnCambioEstatusConexion) this.fEnCambioEstatusConexion(false)
    }
  }

  async detenerConexionHub() {
    try {
        if (this.signalRConexionHub) {
            // Check if the connection is active
          this.signalRConexionHub.stop();
        }
    } catch (err) {
        console.error("Error stopping SignalR connection:", err);
    } finally {
      this.signalRConexionHub = null
    }
  }
}

export default SignalREstacionNetFramework;
