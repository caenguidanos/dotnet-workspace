import http from 'k6/http';

import { check, sleep } from 'k6';

export const options = {
  stages: [
    { duration: '10s', target: 100 }, // below normal load
    { duration: '1m', target: 100 },
    { duration: '10s', target: 1400 }, // spike to 1400 users
    { duration: '3m', target: 1400 }, // stay at 1400 for 3 minutes
    { duration: '10s', target: 100 }, // scale down. Recovery stage.
    { duration: '3m', target: 100 },
    { duration: '10s', target: 0 },
  ],
};
export default function () {
  let BASE_URL = 'http://localhost:5000';

  let [a, b, c, d] = http.batch([
    ['GET', `${BASE_URL}/product/a492d002-9f5d-4ee7-b120-9a366ac661bc`],
    ['GET', `${BASE_URL}/product/78fd61e0-7c18-40d7-88ed-7783b7bde715`],
    ['GET', `${BASE_URL}/product/9eee61ab-887a-4ce3-99a9-76d668e6bd9e`],
    ['GET', `${BASE_URL}/product/312932a1-2c62-4c53-8d7f-f52a176a768b`],
  ]);

  check(a, {
    "status 404": (r) => r.status === 404,
  });

  check(b, {
    "status 404": (r) => r.status === 404,
  });

  check(c, {
    "status 404": (r) => r.status === 404,
  });

  check(d, {
    "status 404": (r) => r.status === 404,
  });

  sleep(1);
}
