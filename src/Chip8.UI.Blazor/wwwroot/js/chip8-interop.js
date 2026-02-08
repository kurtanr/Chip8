// ── Canvas ──────────────────────────────────────────────────────────────────
let canvas, ctx, imageData;
let dotNetRef;
let animationId = null;

export function initialize(canvasId, dotNet) {
  canvas = document.getElementById(canvasId);
  ctx = canvas.getContext('2d');
  imageData = ctx.createImageData(64, 32);
  dotNetRef = dotNet;
  clearCanvas();
  document.addEventListener('keydown', onKeyDown);
  document.addEventListener('keyup', onKeyUp);
}

export function clearCanvas() {
  const d = imageData.data;
  for (let i = 0; i < d.length; i += 4) {
    d[i] = 169; d[i + 1] = 169; d[i + 2] = 169; d[i + 3] = 255;
  }
  ctx.putImageData(imageData, 0, 0);
}

export function renderFrame(pixelData) {
  const d = imageData.data;
  for (let i = 0; i < 2048; i++) {
    const on = pixelData[i];
    const idx = i * 4;
    // ON = AliceBlue (240,248,255), OFF = DarkGray (169,169,169) — matches WPF
    d[idx]     = on ? 240 : 169;
    d[idx + 1] = on ? 248 : 169;
    d[idx + 2] = on ? 255 : 169;
    d[idx + 3] = 255;
  }
  ctx.putImageData(imageData, 0, 0);
}

// ── Game Loop ──────────────────────────────────────────────────────────────
export function startGameLoop() {
  if (animationId !== null) return;
  function loop() {
    dotNetRef.invokeMethod('OnFrame');
    animationId = requestAnimationFrame(loop);
  }
  animationId = requestAnimationFrame(loop);
}

export function stopGameLoop() {
  if (animationId !== null) {
    cancelAnimationFrame(animationId);
    animationId = null;
  }
}

// ── Keyboard ───────────────────────────────────────────────────────────────
const keyState = new Uint8Array(16);

const KEY_MAP = {
  'Digit1': 0x1, 'Digit2': 0x2, 'Digit3': 0x3, 'Digit4': 0xC,
  'KeyQ':   0x4, 'KeyW':   0x5, 'KeyE':   0x6, 'KeyR':   0xD,
  'KeyA':   0x7, 'KeyS':   0x8, 'KeyD':   0x9, 'KeyF':   0xE,
  'KeyZ':   0xA, 'KeyY':   0xA, 'KeyX':   0x0, 'KeyC':   0xB, 'KeyV': 0xF
};

function onKeyDown(e) {
  const k = KEY_MAP[e.code];
  if (k !== undefined) { keyState[k] = 1; e.preventDefault(); }
}

function onKeyUp(e) {
  const k = KEY_MAP[e.code];
  if (k !== undefined) { keyState[k] = 0; e.preventDefault(); }
}

export function getKeyStateMask() {
  let mask = 0;
  for (let i = 0; i < 16; i++) {
    if (keyState[i]) mask |= (1 << i);
  }
  return mask;
}

// ── Sound (Web Audio API) ──────────────────────────────────────────────────
let audioCtx, oscillator, gainNode;

export function initAudio() {
  if (!audioCtx) {
    audioCtx = new AudioContext();
    oscillator = audioCtx.createOscillator();
    gainNode = audioCtx.createGain();
    oscillator.type = 'sine';
    oscillator.frequency.value = 440;
    gainNode.gain.value = 0;
    oscillator.connect(gainNode);
    gainNode.connect(audioCtx.destination);
    oscillator.start();
  }
  if (audioCtx.state === 'suspended') {
    audioCtx.resume();
  }
}

export function playSound() {
  if (gainNode) gainNode.gain.setTargetAtTime(0.3, audioCtx.currentTime, 0.01);
}

export function stopSound() {
  if (gainNode) gainNode.gain.setTargetAtTime(0, audioCtx.currentTime, 0.01);
}

// ── Cleanup ────────────────────────────────────────────────────────────────
export function dispose() {
  stopGameLoop();
  stopSound();
  try { if (oscillator) oscillator.stop(); } catch { /* already stopped */ }
  try { if (audioCtx) audioCtx.close(); } catch { /* already closed */ }
  document.removeEventListener('keydown', onKeyDown);
  document.removeEventListener('keyup', onKeyUp);
}
