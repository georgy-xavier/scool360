/// <reference path="F:\WINCERON\WINER_SCHOOL_5.0\WINER_SCHOOL_v5 _CODE_06072018\WinEr\js files/clienDataProcessor.js" />
/// <reference path="F:\WINCERON\WINER_SCHOOL_5.0\WINER_SCHOOL_v5 _CODE_06072018\WinEr\js files/clienDataProcessor.js" />
// Version 0.6.2
const version = "0.6.11";
const cacheName = 'Winer-${version}';
self.addEventListener('install', e => {
  const timeStamp = Date.now();
  e.waitUntil(
    caches.open(cacheName).then(cache => {
      return cache.addAll([
        '/',
        '/default.aspx',
      ])
          .then(() => self.skipWaiting());
    })
  );
});

self.addEventListener('activate', event => {
  event.waitUntil(self.clients.claim());
});

self.addEventListener('fetch', event => {
  event.respondWith(
    caches.open(cacheName)
      .then(cache => cache.match(event.request, {ignoreSearch: true}))
      .then(response => {
      return response || fetch(event.request);
    })
  );
});