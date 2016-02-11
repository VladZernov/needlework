DnDInitialize();
loadCanvas("srcImgCanvas", "srcImgCanvasDiv");

loadCanvas("patternImgCanvas", "patternImgCanvasDiv");
fillCanvasText("srcImgCanvas", "src");
fillCanvasText("patternImgCanvas", "Pattern");

fillCanvasWImgFromFileListener();
fillCanvasWImgFromUrlListener();


function DnDInitialize(){

  // Initializes drag and drop for dndHolder div


  var holder = document.getElementById('dndHolder'),
    state = document.getElementById('status');

  if (typeof window.FileReader === 'undefined') {
    state.className = 'fail';
  } else {
    state.className = 'success';
    state.innerHTML = 'File API & FileReader available';
  }
   
  holder.ondragover = function () { this.className = 'hover'; return false; };
  holder.ondragend = function () { this.className = ''; return false; };
  holder.ondrop = function (e) {
    this.className = '';
    e.preventDefault();

    var file = e.dataTransfer.files[0],
        reader = new FileReader();
        
    reader.onload = function (event) {
      fillCanvasWImg("srcImgCanvas", event.target.result);
    };
    reader.readAsDataURL(file);
    dragBoxText = document.getElementById('dragBoxText');
  };
}



function fillCanvasWImg(canvasId, dataURL) {

  // Fills defined canvas with image

  var canvas = document.getElementById(canvasId);
  var context = canvas.getContext('2d');

  context.clearRect(0, 0, canvas.width, canvas.height);

  var imageObj = new Image();
  
  imageObj.onload = function() {
    canvas.width = imageObj.width;
    canvas.height = imageObj.height;

    /*if(canvas.width >= imageObj.width && canvas.height >= imageObj.height)
    {
      context.drawImage(this, (canvas.width - imageObj.width)/2,
     (canvas.height - imageObj.height)/2,
     imageObj.width,
     imageObj.height);
    }
    else
    {
      canvas.width = imageObj.width;
      canvas.height = imageObj.height;
      context.drawImage(this, 0, 0, imageObj.width, imageObj.height);
    }*/
    context.drawImage(this, 0, 0, imageObj.width, imageObj.height);
  };

  imageObj.src = dataURL;
  showBlocks(canvasId);

};

function fillCanvasWImgFromUrlListener(){

  var input = document.getElementById('imgUrl');
  input.addEventListener('change', fillCanvasWImgFromUrl);

  
}

function fillCanvasWImgFromUrl(e){
  var url = document.getElementById("imgUrl").value;
  var img = new Image();
  img.onload = function(){

    var canvas = document.createElement('canvas');

    canvas.width = this.width;
    canvas.height = this.height;

    canvas.getContext('2d').drawImage(img, 0, 0);

    var dataURL = canvas.toDataURL();
    console.log(dataURL);
    fillCanvasWImg("srcImgCanvas", dataURL);    
  }

  img.setAttribute('crossOrigin', 'anonymous');
  img.src = url; 
}



function fillCanvasWImgFromFileListener(){
  var input = document.getElementById('fileImg');
  input.addEventListener('change', fillCanvasWImgFromFile);
}

function fillCanvasWImgFromFile(e) {

  var ctx = document.getElementById('srcImgCanvas').getContext('2d');
  var img = new Image;
  img.onload = function() {
    fillCanvasWImg("srcImgCanvas", img.src);
  }
  img.src = URL.createObjectURL(e.target.files[0]);
}


function loadCanvas(canvasId, canvasDiv){
  var canvas = document.getElementById(canvasId);
  var context = canvas.getContext('2d');
  var border = parseInt(getComputedStyle(document.getElementById(canvasDiv),null).getPropertyValue('border-Width'));
  canvas.width = document.getElementById(canvasDiv).offsetWidth - border*2 - 30;
  canvas.height = document.getElementById(canvasDiv).offsetHeight - border*2 - 30;

  //canvasAddMouseZoom(canvas, canvasDiv);


}


function canvasAddMouseZoom(canvas, canvasDiv)
{
  
  var cvsDiv = document.getElementById(canvasDiv);
  //cvsDiv.onwheel.preventDefault();
  canvas.onmousewheel = function() {
  if (canvas.addEventListener) {
    if ('onwheel' in document) {
      // IE9+, FF17+, Ch31+
      canvas.addEventListener("wheel", onWheel);
    } else if ('onmousewheel' in document) {
      // устаревший вариант события
      canvas.addEventListener("mousewheel", onWheel);
    } else {
      // Firefox < 17
      canvas.addEventListener("MozMousePixelScroll", onWheel);
    }
  } else { // IE8-
    canvas.attachEvent("onmousewheel", onWheel);
  }

  }
}

function zoomCanvas(canvasId, zoomX, zoomY){
  var canvas = document.getElementById(canvasId);
  var context = canvas.getContext('2d');
  //context.scale(3, 3);

  /*var imgUrl = canvas.toDataURL();


  var imageObj = new Image();
  imageObj.src = imgUrl;

  canvas.width *= zoomX;
  canvas.height *= zoomX;
  context.drawImage(imageObj, 0, 0);
  context.scale(3, 3);*/
    
}

function onWheel(e) {

  console.log(e);
  if(e.deltaY < 0) zoomCanvas(e.srcElement.id, 1.05, 1.05);
  else zoomCanvas(e.srcElement.id, 0.95, 0.95);

}

function fillCanvasText(canvasId, text){

  // Fills defined canvas with text

  var canvas = document.getElementById(canvasId);
  var context = canvas.getContext('2d');
  context.font = "32px Arial";
  context.fillText(text, canvas.width/2, canvas.height/2 );
}
function showBlocks(canvasId){
  if(canvasId == "srcImgCanvas")
  {
    showDiv("patternSettings");
    showDiv("srcImgCanvasDiv");
  }
  if(canvasId == "patternImgCanvas")
  {
    showDiv("patternImgCanvasDiv");
    showDiv("patternCreated");
  }
}
function showDiv(divId){
  var div = document.getElementById(divId);
  //console.log(divId);
  div.style.visibility = 'visible';
}

function createPattern(){

  console.log("check");
  showBlocks("patternImgCanvas");
  console.log("check");
}

function getAvgColors(){
  // Gets average colors from source image fore each stitch
  var canvas = document.getElementById("srcImgCanvas");
  var ctx = canvas.getContext('2d');

  var stitchSize = getStitchSize();
  console.log("Stitch size: " + stitchSize);
  var imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
  console.log("Source ImageData: ");
  console.log(imageData);
  
  var newImgData = new ImageData(Math.trunc(canvas.width/stitchSize), Math.trunc(canvas.height/stitchSize));
  var imgDataFull = new ImageData(imageData.width, imageData.height);
  //newImgData.data = newPix;
  

  var R=G=B=0;
  //var step = stitchSize*4;
  var blockSize = stitchSize*stitchSize
  for(var i = 0; i < imageData.height; i += stitchSize)
  {
    for(var j = 0; j < imageData.width; j += stitchSize)
    {
      R=G=B=0;
      for(var k = 0; k < blockSize; k++)
      {
        var curPos = (i + Math.trunc(k/stitchSize))*imageData.width*4 + (j + k%stitchSize)*4;
        //console.log(imageData.data[(i + Math.trunc(k/stitchSize))*imageData.width*4 + (j + k%stitchSize)*4]);
        R += imageData.data[curPos];
        G += imageData.data[curPos + 1];
        B += imageData.data[curPos + 2];
        //console.log(curPos);
      }
      R = Math.trunc(R/blockSize);
      G = Math.trunc(G/blockSize);
      B = Math.trunc(B/blockSize);

      for(var k = 0; k < blockSize; k++)
      {
        
        var curPos = (i + Math.trunc(k/stitchSize))*imageData.width*4 + (j + k%stitchSize)*4;
        imgDataFull[curPos] = R;
        imgDataFull[curPos + 1] = R;
        imgDataFull[curPos + 2] = B;
        imgDataFull[curPos + 3] = 0;
      }
      newImgData.data[Math.trunc(i/stitchSize)*4 + Math.trunc(j/stitchSize)*4] = R;
      newImgData.data[Math.trunc(i/stitchSize)*4 + Math.trunc(j/stitchSize)*4 + 1] = G;
      newImgData.data[Math.trunc(i/stitchSize)*4 + Math.trunc(j/stitchSize)*4 + 2] = B;
      newImgData.data[Math.trunc(i/stitchSize)*4 + Math.trunc(j/stitchSize)*4 + 3] = 255;
      //console.log(Math.trunc(i/stitchSize)*4 + Math.trunc(j/stitchSize)*4 + 3);
      //console.log("R: " + Math.trunc(R/(stitchSize*stitchSize)) + " G: " + Math.trunc(G/(stitchSize*stitchSize)) + " B: "+ Math.trunc(B/(stitchSize*stitchSize)));
      //console.log("R: " + R + " G: " + G + " B: " +B);
    }
    //console.log(i*imageData.width*4);
    
  }
  console.log("New image data in one pixs: ");
  console.log(newImgData);
  console.log("New image in full size: ");
  console.log(imgDataFull);

  ctx.putImageData(imgDataFull, 0, 0);
  console.log(ctx.getImageData(0, 0, canvas.width, canvas.height));


}

function getAvgForBlock(startPos, width, height, blockSize){



}
function getStitchSize(){
  // Gets stitch size in pixels for current source picture

  var canvas = document.getElementById("srcImgCanvas");
  var stitchesCount = Math.trunc(document.getElementById("stitchesCount").value);
  var part = canvas.width/stitchesCount%1;
  var size = part<0.5 ? Math.trunc(canvas.width/stitchesCount) : Math.trunc(canvas.width/stitchesCount+1);

  return size;
}