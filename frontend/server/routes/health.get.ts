export default defineEventHandler((event) => {
  // Set cache prevention headers
  setHeaders(event, {
    'Content-Type': 'text/plain',
    'Cache-Control': 'no-store, no-cache',
    'Pragma': 'no-cache',
    'Expires': 'Thu, 01 Jan 1970 00:00:00 GMT',
    'Surrogate-Control': 'no-store',
  });

  return 'Healthy';
});
