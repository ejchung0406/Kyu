using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject bonusBulletPrefab;
    public GameObject curvedBulletPrefab;

    public float bulletForce = 10f;
    public float bonusbulletForce = 2f;

    public int punchDamageAmount = 1;

    public float bonusAtkPoss = 0.8f; // 80% 확률로 보너스 어택

    public float critialPoss = 0.5f;

    public int numBonusbullets = 0;
    public int numCurvedbullets = 4;

    private int maxNumBonusbullets = 5;
    private int maxNumCurvedbullets = 12;

    bool canShoot1 = true;
    float lastShootTime1 = 0f;
    float cooldown1 = 0.8f;

    bool canShoot2 = true;
    float lastShootTime2 = 0f;
    float cooldown2 = 0.8f;
    float coneAngle = 20f;

    bool canShoot3 = true;
    float lastShootTime3 = 0f;
    float cooldown3 = 5f;

    bool canShoot4 = true; // on-off 스킬 켰다 껐다를 켤 수 있는지에 대한 flag
    float lastShootTime4 = 0f;
    float cooldown4 = 10f;
    float autoCooldown4 = 6f; // cooldown4 >= autoCooldown4 왜나면 켜자 마자 바로 공격이 나가니까.
    public bool canAutoShoot4 = false; // 지금 자동으로 나가는 스킬이 on인지 off 인지에 대한 flag
    bool autoShooting4 = false; // 1일때만 발사함. cooldown4 주기로 잠시 켜졌다 바로 꺼짐. 

    private LevelSystem levelSystem;
    private StatSystem statSystem;
    private PlayerHealth playerHealth;

    [SerializeField] private LevelWindow levelWindow;
    [SerializeField] private StatWindow statWindow;

    private void Awake(){
        levelSystem = new LevelSystem();
        levelSystem.AddExperience(0);
        levelWindow.SetLevelSystem(levelSystem);
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;

        statSystem = new StatSystem();
        statSystem.AddAtk(3);
        statSystem.AddDef(10);
        statWindow.SetStatSystem(statSystem);

        playerHealth = GetComponent<PlayerHealth>();
        playerHealth.setupHealth();
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e){
        LevelupShoot();
        statSystem.AddAtk(1);
        if (numBonusbullets < maxNumBonusbullets && levelSystem.getLevel() % 2 == 0) numBonusbullets += 1;
        if (numCurvedbullets < maxNumCurvedbullets && levelSystem.getLevel() % 2 == 0) numCurvedbullets += 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        Shoot4();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && canShoot1){
            Shoot(3);
        }
        if(Input.GetButtonDown("Fire2") && canShoot2){
            Shoot2(3);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canShoot3)
        {
            Shoot3();
        }
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     if (canAutoShoot4) canAutoShoot4 = false; // 끄기는 언제든지 할 수 있음
        //     if (!canAutoShoot4 && canShoot4) Shoot4(); // 켜기
        // }
        if (canAutoShoot4)
        {
            autoShoot4();
        }
    }

    public void enemyHit(int damage, bool triggerBonusAttack){
        if (UnityEngine.Random.value <= bonusAtkPoss && triggerBonusAttack){
            StartCoroutine(BonusShootWithDelay(numBonusbullets));
        }

        playerHealth.Heal(damage * 0.03f);
    }

    void Shoot(int num){
        StartCoroutine(ShootWithDelay(num));

        // Set the cooldown
        canShoot1 = false;
        lastShootTime1 = Time.time;

        // Start the cooldown coroutine for Shoot1
        StartCoroutine(CooldownTimer1());
    }

    void Shoot2(int num){
        Quaternion coneRotation = Quaternion.Euler(0f, 0f, coneAngle / (num-1));
        Vector2 shootDirection = Quaternion.Euler(0f, 0f, -coneAngle / 2) * firePoint.up;

        for (int i = 0; i < num; i++) {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Bullet>().damageAmount = statSystem.getAtk();
            bullet.GetComponent<Bullet>().shooter = this;
            bullet.GetComponent<Bullet>().triggerBonusAttack = true;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(shootDirection * bulletForce, ForceMode2D.Impulse);
            shootDirection = coneRotation * shootDirection;
        }

        // Set the cooldown
        canShoot2 = false;
        lastShootTime2 = Time.time;

        // Start the cooldown coroutine for Shoot1
        StartCoroutine(CooldownTimer2());
    }

    void Shoot4(){
        canAutoShoot4 = true;

        autoShooting4 = true; // 켜자 마자 바로 발사되도록

        canShoot4 = false;
        lastShootTime4 = Time.time;

        // Start the cooldown coroutine for Shoot4
        StartCoroutine(CooldownTimer4());
    }

    void autoShoot4(){
        if (autoShooting4){
            StartCoroutine(ShootCurvedBulletsWithDelay(numCurvedbullets));
            StartCoroutine(CooldownTimerAuto4()); // 바로 autoShooting4 꺼버림. 
        }
    }

    void Shoot3(){
        // Perform shooting logic for Shoot3
        LevelupShoot();

        // Set the cooldown
        canShoot3 = false;
        lastShootTime3 = Time.time;

        // Start the cooldown coroutine for Shoot3
        StartCoroutine(CooldownTimer3());
    }

    void BonusShoot(){
        GameObject bullet = Instantiate(bonusBulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().damageAmount = statSystem.getAtk() / 4;
        bullet.GetComponent<Bullet>().shooter = this;
        bullet.GetComponent<Bullet>().triggerBonusAttack = false;
        bullet.GetComponent<Bullet>().startPosition = this.transform.position;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector3 nearestEnemyPosition = findClosestEnemy(transform.position).position;
        bullet.GetComponent<Bullet>().endPosition = nearestEnemyPosition;
        Vector3 direction = (nearestEnemyPosition - firePoint.position).normalized;
        rb.AddForce(direction * bonusbulletForce, ForceMode2D.Impulse);
    }

    void CurvedShoot(Transform target){
        if (target == null) return;
        GameObject bullet = Instantiate(curvedBulletPrefab, this.transform.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().damageAmount = statSystem.getAtk() / 3;
        bullet.GetComponent<Bullet>().shooter = this;
        bullet.GetComponent<Bullet>().triggerBonusAttack = false;
        bullet.GetComponent<Bullet>().startPosition = this.transform.position;
        bullet.GetComponent<Bullet>().endPosition = target.position;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    }

    private IEnumerator ShootWithDelay(int num) {
        for (int i = 0; i < num; i++) {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Bullet>().damageAmount = statSystem.getAtk();
            bullet.GetComponent<Bullet>().shooter = this;
            bullet.GetComponent<Bullet>().triggerBonusAttack = true;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.10f);
        }
    }

    private IEnumerator BonusShootWithDelay(int num) {
        for (int i = 0; i < num; i++) {
            BonusShoot();
            yield return new WaitForSeconds(0.15f);
        }
    }

    private IEnumerator ShootCurvedBulletsWithDelay(int num) {
        for (int i = 0; i < num; i++) {
            CurvedShoot(findClosestEnemy(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            yield return new WaitForSeconds(0.05f);
        }
    }

    void LevelupShoot(){
        int numBullets = 18;
        float angleStep = 360f / numBullets;

        for (int i = 0; i < numBullets; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            Vector2 direction = rotation * firePoint.up;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            bullet.GetComponent<Bullet>().damageAmount = statSystem.getAtk() / 3;
            bullet.GetComponent<Bullet>().shooter = this;
            bullet.GetComponent<Bullet>().triggerBonusAttack = true;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * bulletForce / 2, ForceMode2D.Impulse);
        }
    }

    public void enemyDied(int experience){
        levelSystem.AddExperience(experience);
    }

    public StatSystem getStatSystem(){
        return statSystem;
    }

    private Transform findClosestEnemy(Vector3 position){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(position, enemy.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }

    IEnumerator CooldownTimer1()
    {
        yield return new WaitForSeconds(cooldown1);

        // Reset the canShoot flag for Shoot1 after the cooldown duration
        canShoot1 = true;
    }

    IEnumerator CooldownTimer2()
    {
        yield return new WaitForSeconds(cooldown2);

        // Reset the canShoot flag for Shoot2 after the cooldown duration
        canShoot2 = true;
    }

    IEnumerator CooldownTimer3()
    {
        yield return new WaitForSeconds(cooldown3);

        // Reset the canShoot flag for Shoot3 after the cooldown duration
        canShoot3 = true;
    }
    IEnumerator CooldownTimer4()
    {
        // Wait for the cooldown duration
        yield return new WaitForSeconds(cooldown4);

        // Enable shooting again
        canShoot4 = true;
    }
    IEnumerator CooldownTimerAuto4()
    {
        autoShooting4 = false;
        // Wait for the cooldown duration
        yield return new WaitForSeconds(autoCooldown4);

        // Enable shooting again
        autoShooting4 = true;
    }
}
