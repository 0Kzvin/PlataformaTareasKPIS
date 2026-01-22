export const CreateAndDownloadFile = (blob, fileName, extension = 'xlsx') => {
  const fileURL = window.URL.createObjectURL(new Blob([blob]));
  const fileLink = document.createElement("a");

  fileLink.href = fileURL;
  fileLink.setAttribute(
    "download",
    `${fileName}.${extension}`
  );

  document.body.appendChild(fileLink);
  fileLink.click();
  document.body.removeChild(fileLink);
  window.URL.revokeObjectURL(fileURL);

}
