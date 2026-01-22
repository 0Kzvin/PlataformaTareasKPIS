//TODO: Testear de manera intensiva el errorCatcher para asegurar estructura a mostrar
export const catchError = (httpErr) => {
    
    const errorObj = {
        exito: false,
    }

    if (httpErr.response) {
        errorObj.statusCode = httpErr.response.status;
        switch (httpErr.response.status) {
            case 401:
                errorObj.payload = {
                    errores: [httpErr.message]
                }
                break;
            case 500:
                errorObj.payload = {
                    errores: [httpErr.message]
                }
                break;
            default:
                errorObj.payload = httpErr.response.data;
                break;
        }
        return errorObj;
    }
    
    if(httpErr?.constructor?.name === "AxiosError") {
        errorObj.payload = {
            errores: [ httpErr.message ]
        };

        return errorObj;
    }

    errorObj.payload = {
        errores: [httpErr]
    };
    return errorObj;
}
