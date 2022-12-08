#!/bin/bash
set -e

psql -U root -d ecommerce --single-transaction \
    -f /var/lib/sql/denifitions/Base.sql \
    -f /var/lib/sql/denifitions/Event.sql \
    -f /var/lib/sql/denifitions/Product.sql
