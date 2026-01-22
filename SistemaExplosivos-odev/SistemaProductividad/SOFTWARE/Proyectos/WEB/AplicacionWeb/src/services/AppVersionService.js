const esVersionMayor = (savedVersion, currentVersion) => {
  const savedVersionArray = savedVersion.split(".");
  const currentVersionArray = currentVersion.split(".");

  return currentVersionArray.some((element, index) => {
    return parseInt(element) > parseInt(savedVersionArray[index]);
  });
};

const limpiarCaches = async () => {
  try {
    // Limpiar caché de la aplicación
    const caches = await window.caches.keys();

    if (caches.length === 0) {
      return;
    }

    for (const cache of caches) {
      await window.caches.delete(cache);
    }

    // Limpiar localStorage completamente
    //  localStorage.clear();

    // Limpiar sessionStorage
    //  sessionStorage.clear();

    // Limpiar IndexedDB (si se usa)
    //  if (window.indexedDB) {
    //    const dbs = await window.indexedDB.databases();
    //    dbs.forEach(db => {
    //      if (db.name) {
    //        window.indexedDB.deleteDatabase(db.name);
    //      }
    //    });
    //  }

    // Limpiar cookies (opcional)
    // document.cookie.split(';').forEach(cookie => {
    //   const eqPos = cookie.indexOf('=');
    //   const name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
    //   document.cookie = `${name}=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/`;
    // });

    // Limpiar service worker (si se usa)
    // if ('serviceWorker' in navigator) {
    //   const registrations = await navigator.serviceWorker.getRegistrations();
    //   await Promise.all(registrations.map(reg => reg.unregister()));
    // }

    window.location.reload(true);
  } catch (error) {
    console.error("Error al limpiar caches:", error);
  }
};

const verificarVersion = async () => {
  const versionStorage = localStorage.getItem("appVersion");

  if (!versionStorage) {
    localStorage.setItem("appVersion", process.env.appVersion);
    await limpiarCaches();
    return;
  }

  if (esVersionMayor(versionStorage, process.env.appVersion)) {
    localStorage.setItem("appVersion", process.env.appVersion);
    await limpiarCaches();
  }
};

const obtenerVersionApp = () => {
  return localStorage.getItem("appVersion");
};

export {
  verificarVersion,
  obtenerVersionApp
}
