#!/bin/bash
set -e

psql -U root \
    -f /var/lib/sql/definitions/Database/Ecommerce.sql
    
psql -U root -d ecommerce --single-transaction \
    -f /var/lib/sql/definitions/Table/Base.sql \
    -f /var/lib/sql/definitions/Table/Product.sql
