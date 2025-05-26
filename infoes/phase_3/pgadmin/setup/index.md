# Docker kurulum kontrolü

- dpkg -l | grep pgadmin

---

## Docker kurulum

  1. url|ca-certificates|gnupg paketlerinin olması gerekir;

```text
➜  ~ dpkg -l | grep -E 'curl|ca-certificates|gnupg'

ii  ca-certificates                               20240203                                 all          Common CA certificates
ii  curl                                          8.5.0-2ubuntu10.6                        amd64        command line tool for transferring data with URL syntax
ii  gnupg                                         2.4.4-2ubuntu17.2                        all          GNU privacy guard - a free PGP replacement
ii  gnupg-l10n                                    2.4.4-2ubuntu17.2                        all          GNU privacy guard - localization files
ii  gnupg-utils                                   2.4.4-2ubuntu17.2                        amd64        GNU privacy guard - utility programs
ii  libcurl3t64-gnutls:amd64                      8.5.0-2ubuntu10.6                        amd64        easy-to-use client-side URL transfer library (GnuTLS flavour)
ii  libcurl4t64:amd64                             8.5.0-2ubuntu10.6                        amd64        easy-to-use client-side URL transfer library (OpenSSL flavour)

```

  2. 