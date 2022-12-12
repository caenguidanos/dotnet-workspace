import http from 'k6/http';

import { check } from 'k6';

export const options = {
  vus: 10,
  duration: '1m',
};

export default function () {
  const BASE_URL = 'http://localhost:5000';

  let res = http.get(`${BASE_URL}/product/8c1c3d09-5783-4a50-8c57-38742415b40d`);

  check(res, {
    'status 404': (r) => r.status === 404
  })
}